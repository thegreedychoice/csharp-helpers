using System;
using System.Collections.Generic;

namespace WordCounter.ModernApp.Data.Model
{
    public partial class BookFeed
    {
        public BookFeed()
        {
            this.BookLines = new List<BookLine>();
        }

        public int Id { get; set; }
        public string Path { get; set; }
        public int LineCount { get; set; }
        public int WordCount { get; set; }
        public string Name { get; set; }
        public long? ProcessingMilliseconds { get; set; }
        public virtual ICollection<BookLine> BookLines { get; set; }
    }
}
