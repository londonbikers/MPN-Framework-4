using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace MediaPanther.Framework.Caching
{
	/// <summary>
	/// Provides functionality to cache domain objects in memory.
	/// </summary>
	public class CacheManager
	{
		#region members
		private static int _itemCeiling;
        private static readonly List<CacheItem> Cache;
		#endregion

		#region accessors
		/// <summary>
		/// Controls the maximum number of items that should be kept in the cache at any one time.
		/// </summary>
		public static int ItemCeiling { get { return _itemCeiling; } set { _itemCeiling = value; } }
		/// <summary>
		/// The number of items currently in the Cache.
		/// </summary>
		public static int ItemCount { get { return Cache.Count; } }
        public static decimal CacheCapacityUsed { get { return CalculateCapacityUsed(); } }
		#endregion

		#region constructors
		/// <summary>
		/// Creates a new CacheManager object.
		/// </summary>
		static CacheManager()
		{
			// set a default ceiling.
			_itemCeiling = 10000;
			if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MediaPanther.Framework.Caching.MaxItems"]))
				_itemCeiling = Convert.ToInt32(ConfigurationManager.AppSettings["MediaPanther.Framework.Caching.MaxItems"]);

            Cache = new List<CacheItem>();
		}
		#endregion

		#region public methods
		/// <summary>
		/// Adds a new object to the cache.
		/// </summary>
		/// <param name="item">The actual object to store in the cache.</param>
        /// <param name="typeIdentifier">The type identifier for the object being cached, i.e. the object type name. Forms a compound key with the primary or secondary key.</param>
		/// <param name="primaryKey">The numeric primary key for the object.</param>
        /// <param name="secondaryKey">An optional secondary key for the object, e.g. a textual name.</param>
		public static void AddItem(object item, string typeIdentifier, long primaryKey, string secondaryKey)
		{
			if (string.IsNullOrEmpty(typeIdentifier))
				throw new ArgumentNullException();

            lock (Cache)
            {
			    var itemCount = primaryKey > 0 ? Cache.Count(ci => ci.PrimaryKey == primaryKey && ci.TypeIdentifier == typeIdentifier) : Cache.Count(ci => ci.SecondaryKey == secondaryKey && ci.TypeIdentifier == typeIdentifier);
                if (itemCount != 0) return;
                var cacheItem = new CacheItem { TypeIdentifier = typeIdentifier, PrimaryKey = primaryKey, Item = item };
                if (!string.IsNullOrEmpty(secondaryKey))
                    cacheItem.SecondaryKey = secondaryKey;

                // is the cache full?
                if (Cache.Count >= _itemCeiling)
                    RemoveUnpopularItem();

                Cache.Add(cacheItem);
            }
		}

	    /// <summary>
	    /// Removes an object from the cache.
	    /// </summary>
        /// <param name="typeIdentifier">The type of identifier being used.</param>
        /// <param name="primaryKey">The unique-identifier for the item to be removed.</param>
	    public static void RemoveItem(string typeIdentifier, int primaryKey)
		{
			if (typeIdentifier == String.Empty)
                throw new ArgumentNullException("typeIdentifier");

			lock (Cache)
			{
				var itemToRemove = Cache.Find(ci => ci.PrimaryKey == primaryKey && ci.TypeIdentifier == typeIdentifier);
				Cache.Remove(itemToRemove);
			}
		}

        /// <summary>
        /// Removes an object from the cache.
        /// </summary>
        /// <param name="typeIdentifier">The type of identifier being used.</param>
        /// <param name="secondaryKey">The unique-identifier string.</param>
        public static void RemoveItem(string typeIdentifier, string secondaryKey)
        {
			if (typeIdentifier == string.Empty)
                throw new ArgumentNullException("typeIdentifier");

            if (string.IsNullOrEmpty(secondaryKey))
                throw new ArgumentNullException("secondaryKey");

            lock (Cache)
			{
				var itemToRemove = Cache.Find(ci => ci.SecondaryKey == secondaryKey && ci.TypeIdentifier == typeIdentifier);
				Cache.Remove(itemToRemove);
			}
        }

	    /// <summary>
	    /// Collects an object that has been cached previously. Will return null if no such item found.
	    /// </summary>
	    /// <param name="typeIdentifier">The type of unique-identifier being used.</param>
	    /// <param name="primaryKey">The unique-identifier for the item to be found.</param>
	    /// <param name="secondaryKey">The secondary unique-identifier for the item to be found.</param>
	    public static object RetrieveItem(string typeIdentifier, int primaryKey, string secondaryKey)
		{
			if (primaryKey < 1 && secondaryKey == string.Empty)
				throw new ArgumentNullException();

	        if (typeIdentifier == string.Empty)
	            throw new ArgumentNullException("typeIdentifier");

	        CacheItem item;
			lock (Cache)
			{
				item = primaryKey > 0 ? Cache.Find(ci => ci.PrimaryKey == primaryKey && ci.TypeIdentifier == typeIdentifier) : Cache.Find(ci => ci.SecondaryKey == secondaryKey && ci.TypeIdentifier == typeIdentifier);
			}

	        if (item == null)
	            return null;

	        item.RequestCount++;
	        return item.Item;
		}

		/// <summary>
		/// Empties the cache of all items.
		/// </summary>
		public static void FlushCache()
		{
			lock (Cache)
				Cache.Clear();
		}

		/// <summary>
		/// Retrieves the top X amount of most popular CacheItems.
		/// </summary>
		/// <param name="count">The number of items to retrieve.</param>
		public static List<CacheItem> RetrieveTopItems(int count)
		{
			if (count >= Cache.Count)
				count = Cache.Count;

			List<CacheItem> items = null;

			lock (Cache)
			{
				items = (from ci in Cache
						 orderby ci.RequestCount descending
						 select ci).Take(count).ToList();
			}

			return items;
		}
		#endregion

		#region private methods
		/// <summary>
		/// Removes the first unpopular item from the Cache to make room.
		/// </summary>
		private static void RemoveUnpopularItem()
		{
			lock (Cache)
			{
				var item = (from ci in Cache
							orderby ci.RequestCount ascending
							select ci).Take(1).FirstOrDefault();

				Cache.Remove(item);
			}
		}

        private static decimal CalculateCapacityUsed()
        {
            if (ItemCeiling == 0 || ItemCount == 0)
                return 0;

            var ret = (decimal)ItemCount / (decimal)ItemCeiling;
            ret = ret * (decimal)100;

            return ret;
        }
		#endregion
	}
}