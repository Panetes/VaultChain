using Ipfs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrusChain.Storage
{
    public class IpfsWrapper : IDisposable
    {
        private Process _process;
        private string _path;

        public IpfsWrapper()
        {
            try
            {
                _path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            }
            catch (Exception)
            {
                _path = @"D:\";
            }
            
        }

        public void Init()
        {
            try
            {
                StartDaemon();
            }
            catch (Exception)
            {
                try
                {
                    _process = new Process();
                    _process.StartInfo.FileName = string.Format("{0}\\ipfs_daemon.exe", _path);
                    _process.StartInfo.Arguments = "init";
                    _process.StartInfo.CreateNoWindow = true;
                    _process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    _process.Start();
                    _process.WaitForExit();
                    Console.WriteLine("IPFS Daemon Initialized");

                    StartDaemon();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
               
            }

  
        }

        private void StartDaemon()
        {
            foreach (var process in Process.GetProcessesByName("ipfs_daemon"))
            {
                process.Kill();
            }

            _process = new Process();
            _process.StartInfo.FileName = string.Format("{0}\\ipfs_daemon.exe", _path);
            _process.StartInfo.Arguments = "daemon";
            _process.StartInfo.CreateNoWindow= true;
            _process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            _process.Start();

            Thread.Sleep(3000);

            Console.WriteLine("IPFS Daemon Started");
        }

        public async Task<string> Add(string fileName)
        {
            using (var httpClient = new HttpClient() { Timeout = Timeout.InfiniteTimeSpan })
            using (var ipfs = new IpfsClient(new Uri("http://127.0.0.1:5001"), httpClient))
            {
                IpfsStream inputStream = new IpfsStream(fileName, File.OpenRead(fileName));
                try
                {
                    MerkleNode node = await ipfs.Add(inputStream);
                    return (String)node.Hash;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async void Get(string Hash)
        {
            using (var httpClient = new HttpClient() { Timeout = Timeout.InfiniteTimeSpan })
            using (var ipfs = new IpfsClient(new Uri("http://127.0.0.1:5001"), httpClient))
            {
                try
                {
                    Stream outputStream = await ipfs.Cat(Hash);
                    FileStream writeStream = new FileStream(_path + "\\file", FileMode.Create, FileAccess.Write);
                    ReadWriteStream(outputStream, writeStream);

                    Process.Start(_path + "\\file");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
             
            }               
        }

        private void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            int Length = 256;
            Byte[] buffer = new Byte[Length];
            int bytesRead = readStream.Read(buffer, 0, Length);
            while (bytesRead > 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, Length);
            }
            readStream.Close();
            writeStream.Close();
        }

        public void Dispose()
        {
            foreach (var process in Process.GetProcessesByName("ipfs_daemon"))
            {
                process.Kill();
            }
        }
    }
}
