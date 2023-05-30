namespace StatusMicroservice
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddSingleton<IStateStore, StateStore>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<StatusManagerService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("This is a gRPC endpoint. Use a gRPC client to access.");
                });
            });
        }
    }
}
