﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Baseline;
using Microsoft.AspNetCore.Http;

namespace Alba
{
    public class HttpRequestBody
    {
        private readonly ISystemUnderTest _system;
        private readonly HttpContext _parent;

        public HttpRequestBody(ISystemUnderTest system, HttpContext parent)
        {
            _system = system;
            _parent = parent;
        }

        public void XmlInputIs(object target)
        {
            var writer = new StringWriter();

            var serializer = new XmlSerializer(target.GetType());
            serializer.Serialize(writer, target);
            var xml = writer.ToString();
            var bytes = Encoding.UTF8.GetBytes(xml);

            var stream = _parent.Request.Body;
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;

            _parent.Request.ContentType = MimeType.Xml.Value;
            _parent.Accepts(MimeType.Xml.Value);
            _parent.Request.ContentLength = xml.Length;
        }

        public void JsonInputIs(object target)
        {
            string json = _system.ToJson(target);

            JsonInputIs(json);
        }

        public void JsonInputIs(string json)
        {
            writeTextToBody(json);
            _parent.Request.ContentType = MimeType.Json.Value;
            _parent.Accepts(MimeType.Json.Value);
            _parent.Request.ContentLength = json.Length;
        }

        private void writeTextToBody(string json)
        {
            var stream = _parent.Request.Body;

            var writer = new StreamWriter(stream);
            writer.Write(json);
            writer.Flush();

            stream.Position = 0;

            _parent.Request.ContentLength = stream.Length;
        }

        public void WriteFormData<T>(T target) where T : class
        {
            _parent.Request.ContentType(MimeType.HttpFormMimetype);

            var values = new Dictionary<string, string>();

            typeof (T).GetProperties().Where(x => x.CanWrite && x.CanRead).Each(prop =>
            {
                var rawValue = prop.GetValue(target, null);

                values.Add(prop.Name, rawValue?.ToString() ?? string.Empty);
            });

            typeof (T).GetFields().Each(field =>
            {
                var rawValue = field.GetValue(target);

                values.Add(field.Name, rawValue?.ToString() ?? string.Empty);
            });

            //TODO: Is this the real form data length?
            _parent.Request.ContentLength = values.Count;

            _parent.WriteFormData(values);
        }

        public void ReplaceBody(Stream stream)
        {
            stream.Position = 0;
            _parent.Request.Body = stream;
        }

        public void TextIs(string body)
        {
            writeTextToBody(body);
            _parent.Request.ContentType = MimeType.Text.Value;
            _parent.Request.ContentLength = body.Length;
        }
    }
}