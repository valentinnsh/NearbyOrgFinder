using MyGeoApplication;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<MyGeoAppStartup>();
    })
    .Build();

host.Run();