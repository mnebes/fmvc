namespace fmvc.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open System.Diagnostics

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

open fmvc.Models

type HomeController(logger: ILogger<HomeController>) =
    inherit Controller()

    member this.Index() =
        let vm: HomeViewModel = { Id = 23 }
        this.View("Index", vm)

    member this.Privacy() = this.View()

    [<ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)>]
    member this.Error() =
        let reqId =
            if isNull Activity.Current then
                this.HttpContext.TraceIdentifier
            else
                Activity.Current.Id

        this.View({ RequestId = reqId })
