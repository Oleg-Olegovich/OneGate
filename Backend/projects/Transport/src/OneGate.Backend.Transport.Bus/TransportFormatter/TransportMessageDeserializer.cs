﻿using System;
using System.IO;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.Text;
using GreenPipes;
using MassTransit;
using Newtonsoft.Json;

namespace OneGate.Backend.Transport.Bus.TransportFormatter
{
    public class TransportMessageDeserializer : IMessageDeserializer
    {
        public static ContentType ContentTypeValue => TransportMessageSerializer.ContentTypeValue;
        public ContentType ContentType => ContentTypeValue;

        private readonly JsonSerializer _deserializer;

        public TransportMessageDeserializer(JsonSerializer deserializer)
        {
            _deserializer = deserializer;
        }

        public void Probe(ProbeContext context)
        {
            var scope = context.CreateScope("json");
            scope.Add("contentType", TransportMessageSerializer.ContentTypeValue);
        }

        public ConsumeContext Deserialize(ReceiveContext receiveContext)
        {
            try
            {
                var messageEncoding = GetMessageEncoding(receiveContext);

                using var body = receiveContext.GetBodyStream();
                using var reader = new StreamReader(body, messageEncoding, false, 1024, true);
                using var jsonReader = new JsonTextReader(reader);

                var messageToken = _deserializer.Deserialize<TransportEnvelope>(jsonReader);
                return new TransportConsumeContext(_deserializer, receiveContext, messageToken);
            }
            catch (JsonSerializationException ex)
            {
                throw new SerializationException(
                    "A JSON serialization exception occurred while deserializing the message", ex);
            }
            catch (SerializationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SerializationException("An exception occurred while deserializing the message", ex);
            }
        }

        private static Encoding GetMessageEncoding(ReceiveContext receiveContext)
        {
            var contentEncoding = receiveContext.TransportHeaders.Get("Content-Encoding", default(string));
            return string.IsNullOrWhiteSpace(contentEncoding) ? Encoding.UTF8 : Encoding.GetEncoding(contentEncoding);
        }
    }
}