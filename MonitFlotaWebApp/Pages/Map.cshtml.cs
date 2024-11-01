using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MonitFlotaWebApp.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

public class MapModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpClientFactory _clientFactory;

    public MapModel(ApplicationDbContext context, IHttpClientFactory clientFactory)
    {
        _context = context;
        _clientFactory = clientFactory;
    }

    public async Task<IActionResult> OnGetRouteDataAsync()
    {
        var jobs = _context.Jobs
            .AsEnumerable()
            .Select(job => new
            {
                id = job.Id,
                service = job.Service,
                delivery = job.Delivery.Split(',').Select(int.Parse).ToArray(),
                location = job.Location.Split(',').Select(double.Parse).ToArray(),
                skills = job.Skills.Split(',').Select(int.Parse).ToArray()
            }).ToList();

        var vehicles = _context.Vehicles
            .AsEnumerable()
            .Select(vehicle => new
            {
                id = vehicle.Id,
                profile = vehicle.Profile,
                start = vehicle.Start.Split(',').Select(double.Parse).ToArray(),
                end = vehicle.End.Split(',').Select(double.Parse).ToArray(),
                capacity = vehicle.Capacity.Split(',').Select(int.Parse).ToArray(),
                skills = vehicle.Skills.Split(',').Select(int.Parse).ToArray(),
                time_window = vehicle.TimeWindow.Split(',').Select(int.Parse).ToArray(),
            }).ToList();

        var jsonContent = new { jobs = jobs, vehicles = vehicles, options = new { g = true } };

        var client = _clientFactory.CreateClient();
        client.BaseAddress = new Uri("https://api.openrouteservice.org");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "5b3ce3597851110001cf624851bc34eb78cc4304b08f2d04d3ea5700");

        var content = new StringContent(JsonConvert.SerializeObject(jsonContent), Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/optimization", content);
        response.EnsureSuccessStatusCode();

        var responseData = await response.Content.ReadAsStringAsync();
        var jsonResponse = JObject.Parse(responseData);

        return new JsonResult(new { routes = jsonResponse["routes"] });
    }
}
