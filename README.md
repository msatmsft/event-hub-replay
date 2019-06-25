# Event Hub Replay
Solution architecture and source code for azure event hub message reply using event hub capture to azure storage account

Often times distributed systems need to perfrom replay of events that happend in past. One of many reasons could be re-processing events with new business logic.

`Event Hubs Standard tier currently supports a maximum retention period of 7 (seven) days. If you need to replay the event beyond standard retention period, following  two patterns emerges.`

### Event Replay Pattern 1 

In this pattern, essentially we are going back to event publishers and re-publishing the events. Events pass through the rest of the pipleline and now you have applied updated business logic (in event consumer, e.g. azure function) to the event data. Event Producers can be other stream processing platforms like apache kafka or apps.

![alt text](https://raw.githubusercontent.com/msatmsft/event-hub-replay/master/img/replay_pattern1.JPG)

` There is no source code for pattern 1 as it explains the idea conceptually.`

### Event Replay Pattern 2

In pattern 2 we are enabling Event Hub feature called Capture. It automatically captures the streaming data in Azure Blob storage of your choice with flexibility of specifying a time or size interval.Captured data is written in Apache Avro format: a compact, fast, binary format that provides rich data structures with inline schema.

For complete details about event hub capture please read documentation [here.](https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-capture-overview)

`Enabling Capture incurs a charge based on your purchased throughput units.`

![alt text](https://raw.githubusercontent.com/msatmsft/event-hub-replay/master/img/replay_pattern2.JPG)

` The source code for pattern 2 is located in \src.`
