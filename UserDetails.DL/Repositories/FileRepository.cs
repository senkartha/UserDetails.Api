
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using UserDetailsDL.Interfaces;

namespace UserDetailsDL.Repositories
{
    public class FileRepository<T> : IRepository<T>
    {
        private readonly IConfiguration _config;

        public FileRepository(IConfiguration config)
        {
            _config = config;
        }

        private static Queue<string> _queue = new Queue<string>();


        public async Task<bool> Save(T obj)
        {
            string filePath = this._config["AppSettings:DataSource"];
            await this.ValidateAndCreateDataSource(filePath);
            string jsonSerializedString = JsonSerializer.Serialize(obj);
            bool arrayAdded = false;
            bool isJsonArray = false;
            int lineNumber = 1;
            using (var strReadWrite = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 4096, true))
            {
                using (var sr = new StreamReader(strReadWrite))
                {
                    string? line;
                    while (true)
                    {
                        line = await sr.ReadLineAsync();
                        if (line == null)
                        {
                            this.WriteToLocation(strReadWrite, 0, jsonSerializedString ?? "");
                            break;
                        }
                        else if (line != null && lineNumber == 1)
                        { // First Line
                            if (!line.StartsWith("["))
                            {
                                this.WriteToLocation(strReadWrite, 0, "[\n" + line);
                                strReadWrite.Seek(2, SeekOrigin.Begin);
                                arrayAdded = true;
                            }
                            isJsonArray = true;
                        }
                        else if (sr.EndOfStream)
                        {
                            if (isJsonArray)
                            {
                                this.WriteToLocation(strReadWrite,
                                    arrayAdded ? strReadWrite.Length : strReadWrite.Length - 2,
                                    ",\n" + (jsonSerializedString ?? "") + "\n]");
                            }

                            break;
                        }
                        lineNumber++;
                    }
                }
            }

            return true;
        }

        private void WriteToLocation(FileStream fs, long position, string data)
        {
            fs.Seek(position, SeekOrigin.Begin);
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            fs.Write(buffer, 0, buffer.Length);
        }


        /// <summary>
        /// Method to create the File data source
        /// </summary>
        /// <param name="fileName">File full name</param>
        /// <returns>A status indicating whether the data source is valid</returns>
        public async Task<bool> ValidateAndCreateDataSource(string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    using (var fs = new FileStream(fileName, FileMode.OpenOrCreate,
                        FileAccess.Write, FileShare.None, 0, true))
                    {
                        await fs.WriteAsync(new byte[0], 0, 0);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

