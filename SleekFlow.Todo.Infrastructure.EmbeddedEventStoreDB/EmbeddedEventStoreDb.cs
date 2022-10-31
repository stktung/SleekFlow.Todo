﻿using EventStore.ClientAPI;
using EventStore.ClientAPI.Embedded;
using EventStore.Core;

namespace SleekFlow.Todo.Infrastructure.EmbeddedEventStoreDB
{
    public class EmbeddedEventStoreDb
    {
        private static readonly TimeSpan TimeToStop = TimeSpan.FromSeconds(5);

        private ClusterVNode _node;

        public EmbeddedEventStoreDb()
        {
            var nodeBuilder = EmbeddedVNodeBuilder
                .AsSingleNode()
                .OnDefaultEndpoints()
                .RunInMemory();

            _node = nodeBuilder.Build();
            _node.StartAsync(true).Wait();

            Connection = EmbeddedEventStoreConnection.Create(_node);

            Connection.ConnectAsync().Wait();
        }

        public IEventStoreConnection Connection { get; }

        public void Dispose()
        {
            Connection?.Close();
            _node.StopAsync(TimeToStop).Wait();
        }
    }
}