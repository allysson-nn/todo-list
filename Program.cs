using System.Net.Http.Json;

namespace todo_console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7064");
            
            while (true)
            {
                Console.Clear();

                Console.WriteLine("========== Gerenciador de Tarefas ==========\n");

                Console.WriteLine("Escolha uma opção a seguir:\n");

                Console.WriteLine("1. Listar todas as tarefas");
                Console.WriteLine("2. Buscar tarefa por ID");
                Console.WriteLine("3. Criar uma nova tarefa");
                Console.WriteLine("4. Concluir/Alterar status de uma tarefa");
                Console.WriteLine("5. Excluir tarefa");
                Console.WriteLine("0. Sair\n");

                string? option = Console.ReadLine();

                if (option == "0") break;

                switch(option)
                {
                    case "1":
                        await GetAllTasksAsync(client);
                        break;
                    case "2":
                        await GetTaskByIdAsync(client);
                        break;
                    case "3":
                        await CreateTaskAsync(client);
                        break;
                    case "4":
                        await ChangeTaskStatusAsync(client);
                        break;
                    case "5":
                        await DeleteTaskAsync(client);
                        break;
                    default:
                        Console.WriteLine("\nOpção inválida! Pressione qualquer tecla para continuar.");
                        Console.ReadKey();
                        break;
                }
      
            }
   
        }


        // ---------- MÉTODOS DE CONSUMO DA API ----------

        static async Task GetAllTasksAsync(HttpClient client)
        {
            Console.Clear();
            Console.WriteLine("---------- LISTA DE TAREFAS DA API ----------\n");

            try
            {
                var response = await client.GetAsync("api/tasks");

                if (response.IsSuccessStatusCode)
                {
                    var tasks = await response.Content.ReadFromJsonAsync<List<TaskDto>>();

                    if (tasks != null && tasks.Count > 0)
                    {
                        foreach (var task in tasks)
                        {
                            string status = task.Complete ? "[X]" : "[ ]";
                            Console.WriteLine($"\nID: {task.Id} | {status} {task.Title}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nenhuma tarefa encontrada.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao se comunicar com a API: {ex.Message}");
            }

            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
        }

        static async Task GetTaskByIdAsync(HttpClient client)
        {
            Console.Clear();
            Console.WriteLine("---------- BUSCA DE TAREFA POR ID ----------\n");

            Console.Write("Digite o ID da tarefa que deseja buscar: ");


            if (int.TryParse(Console.ReadLine(), out int id)) // O TryParse Tenta ler o que o usuário digitou e converter em número. Se ele realmente digitou um número válido, guarde-o na variável id e entre no bloco if para buscar a tarefa
            {
                try
                {
                    var response = await client.GetAsync($"api/tasks/{id}"); // Faz um GET passando o ID diretamente na rota

                    if (response.IsSuccessStatusCode)
                    {
                        var task = await response.Content.ReadFromJsonAsync<TaskDto>();

                        if (task != null)
                        {
                            Console.WriteLine("\n----- Tarefa encontrada com sucesso! -----\n");

                            string status = task.Complete ? "[X]" : "[ ]";
                            Console.WriteLine($"ID: {task.Id} | {status} | {task.Title}");
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound) // Se a API retornar 404 Not Found (Tarefa não existe)
                    {
                        Console.WriteLine($"\nAtenção: Nenhuma tarefa foi encontrada com o ID {id} (404 Not Found).");
                    }
                    else
                    {
                        Console.WriteLine($"\nOcorreu um erro inesperado. Status Code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nErro de conexão com a API: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("\nID inválido! Digite apenas números inteiros.");
            }

            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
        }

        private static async Task CreateTaskAsync(HttpClient client)
        {
            Console.Clear();
            Console.WriteLine("---------- CRIAR TAREFA ----------\n");

            Console.WriteLine("Digite o título da tarefa: ");

            using var reader = new System.IO.StreamReader(Console.OpenStandardInput(), System.Text.Encoding.UTF8);

            string? taskTitle = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(taskTitle))
            {
                try
                {
                    var newTask = new { Title = taskTitle };

                    var response = await client.PostAsJsonAsync("api/tasks", newTask);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("\nTarefa criada com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine($"\nErro ao criar a tarefa. Status: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nErro de conexão com a API: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("\nO título da tarefa não pode estar vazio!");
                Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
        }

        private static async Task ChangeTaskStatusAsync(HttpClient client)
        {
            Console.Clear();
            Console.WriteLine("---------- ALTERAR STATUS DA TAREFA ----------\n");

            Console.Write("Digite o ID da tarefa que deseja alterar o status: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    var response = await client.PatchAsync($"api/tasks/{id}/status", null);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("\nStatus da tarefa alterado com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine($"\nErro ao alterar o Status da tarefa. Status: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nErro de conexão com a API: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("\nID inválido! Digite apenas números inteiros.");
            }

            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
        }

        private static async Task DeleteTaskAsync(HttpClient client)
        {
            Console.Clear();
            Console.WriteLine("---------- DELETAR TAREFA ----------\n");

            Console.Write("Digite o ID da tarefa que deseja excluir: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    var response = await client.DeleteAsync($"api/tasks/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Tarefa deletada com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("\nTarefa não encontrada.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nErro de conexão com a API: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("\nID inválido! Digite apenas números inteiros.");
            }

            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
        }

        // Data Transfer Object (DTO): Um DTO serve para transportar dados entre diferentes partes de um sistema de forma leve e segura, isolando a estrutura interna do banco de dados daquilo que é enviado para o cliente ou tela.
        public class TaskDto 
        {
            public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public bool Complete { get; set; }
        }
    }
}