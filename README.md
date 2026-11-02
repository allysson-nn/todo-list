# Lista de Tarefas
Projeto desenvolvido em C# que consome a `todo-api`, executando as operações completas de CRUD (*Create, Read, Update and Delete*) através do `HttpClient`.

## Como Executar o Projeto

### Pré-requisitos

Antes de executar este projeto, certifique-se de que:

1. O **.NET SDK** esteja instalado na sua máquina. (Esse projeto foi criado e testado na versão 8.0).
2. O repositório da <a href="https://github.com/allysson-nn/todo-api" target="_blank">todo-api</a> esteja clonado, configurado e em execução na sua máquina local.

### Passos

1. **Clonar o Repositório:**
```bash
https://github.com/allysson-nn/todo-api.git
```

2. **Verificar o Endereço da API:** Por padrão, esta aplicação está configurada para consumir a API no endereço base `https://localhost:7064`. Caso a porta da sua API local seja diferente (configurada no arquivo `Properties/launchSettings.json` do projeto da API), certifique-se de ajustar o endereço no código deste console app antes de executá-lo, para que a comunicação funcione corretamente.

3. **Executar a Aplicação:** Com a API rodando em background, execute o console app com
```bash
dotnet run
```

## Interface

<p align="center">
  <img src="https://github.com/user-attachments/assets/f1803e2f-3eb1-4c8b-be66-da9b79bf22ff" alt="Console App (Tela inicial)" width="450">
</p>

<p align="center">
  <img src="https://github.com/user-attachments/assets/63c88d70-8961-45d8-9021-5d08ee9138b3" alt="Console App (Tela de listagem de tarefas)" width="450">
</p>


## Tecnologias Utilizadas no Projeto

* **C# / .NET**
