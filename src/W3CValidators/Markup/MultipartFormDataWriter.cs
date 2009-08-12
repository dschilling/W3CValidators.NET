// Copyright (c) 2009 Daniel A. Schilling

namespace W3CValidators.Markup
{
    using System;
    using System.IO;
    using System.Text;

    internal class MultipartFormDataWriter : IDisposable
    {
        private readonly Stream _stream;
        private readonly string _boundary;
        private readonly string _header;
        private readonly string _footer;

        public MultipartFormDataWriter(Stream stream, string boundary)
        {
            _stream = stream;
            _boundary = boundary;

            _header = string.Concat("--", _boundary);
            _footer = string.Concat("--", _boundary, "--");
        }

        public void Dispose()
        {
            this.WriteLine(_footer);
        }

        public void Write(string name, string value)
        {
            this.WriteLine(_header);
            this.WriteLine(string.Concat("Content-Disposition: form-data; name=\"", name, "\""));
            this.WriteLine();
            this.WriteLine(value);
        }

        public void Write(string name, string filename, string contentType, byte[] data)
        {
            this.WriteLine(_header);
            this.WriteLine(string.Concat("Content-Disposition: form-data; name=\"", name, "\"; filename=\"", filename, "\""));
            this.WriteLine(string.Concat("Content-Type: ", contentType));
            this.WriteLine();
            this.Write(data);
            this.WriteLine();
        }

        private void Write(byte[] value)
        {
            _stream.Write(value, 0, value.Length);
        }

        private void Write(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            this.Write(bytes);
        }

        private void WriteLine()
        {
            this.Write("\r\n");
        }

        private void WriteLine(string value)
        {
            this.Write(value);
            this.WriteLine();
        }
    }
}