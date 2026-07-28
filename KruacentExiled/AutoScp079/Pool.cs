using System.Collections.Generic;

namespace KruacentExiled.AutoScp079
{
    public static class Pool<T> where T : new()
    {

        private static Queue<T> _items = new Queue<T>();

        public static T Get()
        {
            if(!_items.TryDequeue(out T result))
            {
                result = new T();
            }
            return result;
        }


        public static void Return(T item)
        {
            _items.Enqueue(item);
        }
    }
}
