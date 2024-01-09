namespace fmvc

#nowarn "20"

open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpsPolicy
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging

open Westwind.AspNetCore.LiveReload
open fmvc.Models
open fmvc.Users

open Giraffe
open Giraffe.EndpointRouting

open Giraffe.Razor

module Program =
    let exitCode = 0
    
    let model: HomeViewModel = { Id = 12 }
    let giraffeEndpoints = [
        subRoute "/giraffe" [
            GET [
                route "/home" (razorHtmlView "Home/Index" (Some model) None None)
            ]
        ]
    ]

    [<EntryPoint>]
    let main args =
        let builder = WebApplication.CreateBuilder(args)
        
        #if DEBUG
        builder.Services.AddLiveReload()
        #endif

        builder
            .Services
            .AddControllersWithViews()
            .AddRazorRuntimeCompilation()

        builder.Services.AddRazorPages()
        
        builder.Services.AddTransient<NameService>()

        let app = builder.Build()
        
        #if DEBUG
        app.UseLiveReload()
        #endif

        if not (builder.Environment.IsDevelopment()) then
            app.UseExceptionHandler("/Home/Error")
            app.UseHsts() |> ignore // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

        app.UseHttpsRedirection()

        app.UseStaticFiles()
        app.UseRouting()
        app.UseAuthorization()
        
        app.MapGiraffeEndpoints(giraffeEndpoints)

        app.MapControllerRoute(name = "default", pattern = "{controller=Home}/{action=Index}/{id?}")

        app.MapRazorPages()

        app.Run()

        exitCode
