using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TaskManager.Shared;

namespace TaskManager.Client.Pages
{
    public partial class EditTodo
    {
        [Inject] public HttpClient HttpClient { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        [Parameter] public Guid Id { get; set; }

        private Todo _todo = new();

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (Id != Guid.Empty)
            {
                await LoadTodoAsync();
            }
        }

        private async Task LoadTodoAsync()
        {
            var todoResponse = await HttpClient.GetAsync($"todo/get/{Id}");

            if (todoResponse.IsSuccessStatusCode)
            {
                string content = await todoResponse.Content.ReadAsStringAsync();

                _todo = JsonConvert.DeserializeObject<Todo>(content);
            }
            else
            {
                NavigationManager.NavigateTo("/");
            }
        }

        private async Task HandleValidSubmitAsync()
        {
            string content = JsonConvert.SerializeObject(_todo);

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            _ = await HttpClient.PostAsync("todo/update", byteContent);

            _todo = new();

            NavigationManager.NavigateTo("/");
        }
    }
}
