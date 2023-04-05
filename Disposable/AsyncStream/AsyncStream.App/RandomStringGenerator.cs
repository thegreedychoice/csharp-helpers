using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AsyncStream.App
{
    public class RandomStringGenerator : IAsyncDisposable
    {
        private MemoryStream _buffer = new(100 * 1024 * 1024);

        public RandomStringGenerator()
        {
            Console.WriteLine(" * RandomStringGenerator - created");
        }

        public async IAsyncEnumerable<string> Get(int count)
        {
            await FillBuffer(count);
            for (int i=0; i<count; i++)
            {
                var b = new byte[16];
                await _buffer.ReadAsync(b);
                yield return new Guid(b).ToString();
            }
        }

        private async Task FillBuffer(int count)
        {
            await _buffer.FlushAsync();
            for (int i = 0; i < count; i++)
            {
                var g = Guid.NewGuid();
                await _buffer.WriteAsync(g.ToByteArray());
            }
            _buffer.Position = 0;
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_buffer is not null)
            {
                await _buffer.DisposeAsync().ConfigureAwait(false);
                _buffer = null;
                Console.WriteLine(" * RandomStringGenerator - disposed");
            }
        }
    }
}
