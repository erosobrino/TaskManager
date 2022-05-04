using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TaskManager.Shared;

namespace TaskManager.Client.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject] public HttpClient HttpClient { get; set; }

        private List<Todo> _pendingTasks = new();
        private List<Todo> _finishedTasks = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await LoadFinishedTodosAsync();
            await LoadPendingTodosAsync();
        }

        private async Task LoadPendingTodosAsync()
        {
            var pendingResponse = await HttpClient.GetAsync("todo/pending");

            if (pendingResponse.IsSuccessStatusCode)
            {
                string content = await pendingResponse.Content.ReadAsStringAsync();

                _pendingTasks = JsonConvert.DeserializeObject<List<Todo>>(content);
            }
        }

        private async Task LoadFinishedTodosAsync()
        {
            var finishedResponse = await HttpClient.GetAsync("todo/finished");

            if (finishedResponse.IsSuccessStatusCode)
            {
                string content = await finishedResponse.Content.ReadAsStringAsync();

                _finishedTasks = JsonConvert.DeserializeObject<List<Todo>>(content);
            }
        }



        private void addNewTask()
        {
            //if (NewTaskName != "" && NewTaskDescription != "")
            //{
            //    Task newTask = new(NewTaskName, NewTaskDescription);
            //}
            //else
            //{
            //    //<h4 style="color:red;">No has introducido los datos de la nueva tarea</h4>
            //}
        }
    }
}
