﻿using System;
using System.Collections.Generic;
using System.Linq;
using MassTransit;
using MassTransit.Context;
using MassTransit.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OneGate.Backend.Transport.Bus.TransportFormatter
{
    public class TransportConsumeContext : DeserializerConsumeContext
    {
        private readonly JsonSerializer _deserializer;
        private readonly TransportEnvelope _envelope;
        private readonly JToken _messageToken;

        public TransportConsumeContext(JsonSerializer deserializer, ReceiveContext receiveContext, TransportEnvelope envelope)
            : base(receiveContext)
        {
            _envelope = envelope ?? throw new ArgumentNullException(nameof(envelope));

            _deserializer = deserializer;
            _messageToken = GetMessageToken(envelope.Payload);
        }

        public override bool HasMessageType(Type messageType)
        {
            return _envelope.Contract == TransportExtensions.GetEntityName(messageType);
        }

        public override bool TryGetMessage<T>(out ConsumeContext<T> consumeContext)
        {
            if (!HasMessageType(typeof(T)))
            {
                consumeContext = null;
                return false;
            }

            using var jsonReader = _messageToken.CreateReader();
            var obj = _deserializer.Deserialize(jsonReader, typeof(T));

            consumeContext = new MessageConsumeContext<T>(this, (T) obj);
            return true;
        }

        public override Guid? MessageId => ConvertIdToGuid(_envelope.MessageId);
        public override Guid? RequestId => ConvertIdToGuid(_envelope.RequestId);
        public override Guid? CorrelationId => ConvertIdToGuid(_envelope.CorrelationId);
        public override Guid? ConversationId => ConvertIdToGuid(_envelope.ConversationId);
        public override Guid? InitiatorId => ConvertIdToGuid(_envelope.InitiatorId);
        public override DateTime? ExpirationTime => _envelope.ExpirationTime;
        public override Uri SourceAddress => ConvertToUri(_envelope.SourceAddress);
        public override Uri DestinationAddress => ConvertToUri(_envelope.DestinationAddress);
        public override Uri ResponseAddress => ConvertToUri(_envelope.ResponseAddress);
        public override Uri FaultAddress => ConvertToUri(_envelope.FaultAddress);
        public override DateTime? SentTime => _envelope.SentTime;
        public override Headers Headers => NoMessageHeaders.Instance;
        public override HostInfo Host => default;
        public override IEnumerable<string> SupportedMessageTypes => Enumerable.Empty<string>();

        private static JToken GetMessageToken(object message)
        {
            if (!(message is JToken messageToken) || messageToken.Type == JTokenType.Null)
                return new JObject();
            return messageToken;
        }

        private static Guid? ConvertIdToGuid(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return default;
            if (Guid.TryParse(id, out var messageId))
                return messageId;
            throw new FormatException("The Id was not a Guid: " + id);
        }

        private static Uri ConvertToUri(string uri)
        {
            return string.IsNullOrWhiteSpace(uri) ? null : new Uri(uri);
        }
    }
}