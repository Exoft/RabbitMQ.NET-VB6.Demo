# RabbitMQ VB6 Client
RabbitMQ .net 4 + VB6 Simple client 

### Installation

Docker is required to install RabbitMQ, skip this if RabbitMQ is already installed

```sh
docker run -d -p 5672:5672 -p 8080:15672 --hostname my-rabbit --name rabbit rabbitmq:3-management
```

Build and register VB6ComLib

You need to register this assembly using the **RegAsm.exe** tool (32 bits). This tool is located in your .Net Framework version in **C:\Windows\Microsoft.NET\Framework**.

```sh
C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "C:\Users\IvanShikht\Documents\RabbitMQ Demo\VB6ComLib\VB6ComLib\VB6ComLib\bin\Debug\VB6ComLib.dll" /tlb:"C:\Users\IvanShikht\Documents\RabbitMQ Demo\VB6ComLib\VB6ComLib\VB6ComLib\bin\Debug\VB6ComLib.tlb"
```

To unregister use **/u** instead of **/codebase**.
