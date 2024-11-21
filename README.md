# EcoWatt

## Visão Geral

O **EcoWatt** é uma solução inovadora para monitoramento e gestão de consumo de energia, projetada para atender pequenos negócios que buscam eficiência energética e controle de gastos. A plataforma integra dispositivos IoT para coletar dados de consumo energético e utiliza APIs robustas desenvolvidas em .NET para processar, armazenar e gerar relatórios gerenciais detalhados. A API em .NET é responsável pela criação e manipulação de relatórios agregados que fornecem insights estratégicos para análise de consumo, permitindo uma visão consolidada e prática para a tomada de decisão.

A aplicação se conecta a um banco de dados Oracle para armazenamento de dados históricos e agregados, enquanto utiliza MongoDB para backups e análise de grandes volumes de dados. Seguindo princípios de **Clean Code** e **SOLID**, a arquitetura do EcoWatt prioriza manutenibilidade e escalabilidade, com a aplicação do padrão de design **Singleton** para garantir controle sobre o gerenciamento de relatórios. A solução inclui endpoints CRUD que possibilitam a criação, consulta, atualização e remoção de relatórios e dispositivos, além de integração com CI/CD para deploy contínuo, utilizando o Azure para automação.

## Tecnologias Utilizadas

- **.NET 8.0**: Desenvolvimento da API.
- **Entity Framework Core**: Interação com o banco de dados Oracle.
- **Moq**: Mocking para testes unitários com xUnit.
- **xUnit**: Framework de testes.
- **Oracle**: Banco de dados principal.
- **Azure**: Integração com CI/CD para deploy contínuo.


## Funcionalidades

- **Gestão de Relatórios:**
  - Criação, leitura, atualização e deleção de relatórios.
- **Gestão de Consumos Agregados:**
  - Criação, leitura, atualização e deleção de consumos agregados vinculados a relatórios.
- **Integração com Dispositivos:**
  - Verificação da existência de dispositivos antes de associar consumos.
- **Documentação da API:**
  - Uso de `[ProducesResponseType]` para documentação clara dos endpoints.
- **Testes Unitários:**
  - Implementação de testes com **Moq** e **xUnit** para garantir a qualidade do código.
- **Caching de Configurações:**
  - Utilização do **Design Pattern Singleton** para gerenciamento eficiente de configurações em cache.

## Princípios de Clean Code e SOLID Aplicados

- **Responsabilidade Única (SRP):**
  - Cada classe possui uma única responsabilidade, facilitando a manutenção e evolução do código.
  
- **Aberto/Fechado (OCP):**
  - O sistema está aberto para extensão, mas fechado para modificações, permitindo adicionar novas funcionalidades sem alterar o código existente.
  
- **Substituição de Liskov (LSP):**
  - Classes derivadas substituem suas classes base sem alterar o funcionamento do sistema.
  
- **Segregação de Interfaces (ISP):**
  - Interfaces específicas são criadas para diferentes funcionalidades, evitando a sobrecarga de métodos desnecessários.
  
- **Inversão de Dependência (DIP):**
  - Módulos de alto nível não dependem de módulos de baixo nível; ambos dependem de abstrações.

## Exemplos de Requisições

### Adicionar um Novo Relatório

**Endpoint:** `POST /api/Relatorios`

**Corpo da Requisição:**
```
{
  "nome": "Relatório de Atualização de Consumo"
}
```

### Adicionar um Novo Consumo Agregado

**Endpoint:** `POST /api/ConsumoAgregado`

**Corpo da Requisição:**
```
{
  "RelatorioId": 29,
  "Consumo": 195.60,
  "DataConsumo": "2024-03-20T16:00:00",
  "Descricao": "Novo consumo agregado"
}
```

## Como Executar o Projeto

### Pré-requisitos

- **.NET 8.0 SDK**
- **Oracle Database**

### Passo a Passo

1. **Clonar o Repositório:**
   ```
   git clone https://github.com/andresant-ana/EcoWattAPI.git
   cd EcoWatt/EcoWatt.API
   ```

2. **Configurar as Strings de Conexão:**
   - Edite o arquivo `appsettings.json` com as informações do seu banco de dados Oracle.

3. **Aplicar Migrations:**
   ```
   dotnet ef database update
   ```

4. **Rodar a Aplicação:**
   ```
   dotnet run
   ```

5. **Acessar a Documentação da API:**
   - Navegue até `https://localhost:<porta>/swagger` para visualizar a documentação interativa da API.

## Executando os Testes

1. **Navegue até o Projeto de Testes:**
   ```
   cd ../EcoWatt.Tests
   ```

2. **Executar os Testes com xUnit:**
   ```
   dotnet test
   ```
