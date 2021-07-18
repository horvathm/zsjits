using LockerManagement;
using LockerServices;
using LockerServices.Fakes;
using LockerServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

var sp = new ServiceCollection()
    .AddSingleton<IDatabaseService, DatabaseServiceFake>()
    .AddSingleton<ILockerSystemManager, LockerSystemManagerFake>()
    .AddLogging(x => x.AddConsole())
    .BuildServiceProvider();

LockerManager lockerManager = new LockerManager(
    sp.GetRequiredService<ILockerSystemManager>(),
    sp.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>());

var subscriber1 = new DatabaseServiceAdapter(sp.GetRequiredService<IDatabaseService>());
var subscriber2 = new DatabaseServiceAdapter(sp.GetRequiredService<IDatabaseService>());

// Attach a subscriber
lockerManager.AttachSubscriber(subscriber1);
lockerManager.AttachSubscriber(subscriber2);
lockerManager.DeactivateSubscriber(subscriber2);

// Turn eco mode on
await lockerManager.TurnEcoModeOn();

// Turn eco mode off
await lockerManager.TurnEcoModeOff();

// Detach subscriber
lockerManager.DetachSubscriber(subscriber1);
lockerManager.DetachSubscriber(subscriber2);

Console.ReadKey();