using LockerManagement;
using LockerServices;
using LockerServices.Fakes;
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

var subscriber = new DatabaseServiceAdapter(sp.GetRequiredService<IDatabaseService>());

// Attach a subscriber
lockerManager.AttachSubscriber(subscriber);

// Turn eco mode on
await lockerManager.TurnEcoModeOn();

// Turn eco mode off
await lockerManager.TurnEcoModeOff();

// Detach subscriber
lockerManager.DetachSubscriber(subscriber);

Console.ReadKey();