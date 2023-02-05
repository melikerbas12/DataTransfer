using System.Collections.Generic;

namespace DataTransfer.Console.Entities
{
    public class EntityResult<T> where T : class
    {
        public bool Result { get; set; }
        public string ErrorText { get; set; }
        public T Object { get; set; }
        public List<T> Objects { get; set; }
    }
}