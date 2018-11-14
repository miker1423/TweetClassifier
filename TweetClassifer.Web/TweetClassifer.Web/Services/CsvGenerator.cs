using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using TweetClassifer.Web.Models;

namespace TweetClassifer.Web.Services
{
    public class CsvGenerator
    {
        private const string Comma = ",";
        private const string NewLine = "\n";
        public async static Task<FileStream> GetCSV(List<TweetEntry> entries)
        {
            if (!Directory.Exists(@".\Temp"))
                Directory.CreateDirectory(@".\Temp");

            var builder = new StringBuilder();
            var file = File.Create($@".\Temp\{Guid.NewGuid()}.csv");

            foreach(var entry in entries)
            {
                builder.Append($"\"{entry.Tweet.Text}\"");
                builder.Append(Comma);
                builder.Append(entry.Tweet.Gender);
                builder.Append(NewLine);
            }

            var encoded = Encoding.UTF8.GetBytes(builder.ToString());
            await file.WriteAsync(encoded);
            await file.FlushAsync();
            file.Seek(0, SeekOrigin.Begin);
            return file as FileStream;
        }
    }
}
