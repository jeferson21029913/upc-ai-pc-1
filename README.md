# UpcAiPc1 - ASP.NET Core MVC (.NET 10)

Aplicacion MVC con Clean Architecture y un endpoint `GET /` que responde JSON cargado desde un archivo fijo del proyecto.

## Arquitectura

- `src/UpcAiPc1.Domain`: contratos de dominio (`IJsonSourceRepository`).
- `src/UpcAiPc1.Application`: caso de uso y validacion de JSON.
- `src/UpcAiPc1.Infrastructure`: lectura del archivo JSON desde disco.
- `src/UpcAiPc1.Web`: API MVC y configuracion del host.

## Endpoint

- `GET http://localhost:5000/`
  - Si `src/UpcAiPc1.Web/Data/source.json` es JSON valido, devuelve su contenido.
  - Si el archivo no tiene formato JSON valido, devuelve:
    - `400 Bad Request` con mensaje en espanol.

## Como ejecutar

1. Desde la raiz del repositorio, ejecutar:
   - `dotnet run --project src/UpcAiPc1.Web`
2. Abrir en navegador:
   - `http://localhost:5000`

Con eso basta para probar la aplicacion.
