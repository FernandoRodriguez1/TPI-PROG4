**Trabajo Práctico Integrador – Programación IV**

# Sistema de Gestión de Hinchas y Venta de Entradas
---

## 📌 Descripción

Este proyecto es un **sistema de gestión de hinchas y venta de entradas para partidos de fútbol**, desarrollado siguiendo los principios de **Clean Architecture**, con **ASP.NET Core** y **Entity Framework Core** como base tecnológica.

El sistema ofrece las siguientes funcionalidades principales:

- ✅ **Registro y administración de usuarios** (administradores e hinchas).
- ✅ **Gestión completa de partidos**: creación, edición, eliminación y listado.
- ✅ **Venta de entradas** con control de stock y validación de disponibilidad.
- ✅ **Gestión de socios** mediante carnets de socios.
- ✅ Uso de **DTOs**, servicios y repositorios respetando las buenas prácticas de Clean Architecture.
- ✅ **Seguridad y validaciones**: correos, contraseñas, roles y reglas de negocio.
- ✅ Implementación de **Polly** con **Retry** y **Circuit Breaker** para tolerancia a fallos.
- ✅ Uso de **HttpClient Factory** para consumir APIs externas de forma eficiente.
- ✅ **Manejo global de excepciones** con mensajes estructurados.
- ✅ Lectura de **variables de entorno** mediante Azure.

---
## Tecnologías Utilizadas

Al desarrollar el proyecto, se utilizó varias tecnologías y librerías modernas, algunas son:

- **Automapper:** Nuget para hacer el pasaje automático de los DTO a las entidades.
- **EntityFramework:** ORM para mapear entidades a bases de datos.
- **Microsoft Authentication Bearer** Nuget que nos permite enviar un JWT como bearer.

---
## Instalación

Para instalar y ejecutar la aplicación localmente, debes seguir estos pequeños pasos:

1. Clona este repositorio:
    ```sh
    git clone https://github.com/FernandoRodriguez1/TPI-PROG4.git
    ```

2. Navega al directorio del proyecto:
    ```sh
    cd src
    ```

3. Entrar al proyecto WebApi
   ```sh
    cd MatchTickets.WebApi
    ```
3. Instala las dependencias:
    ```sh
    dotnet build
    ```

4. Inicia la aplicación (Asegurate de modificar los puertos LocalHost, donde correrá la app, por el puerto deseado):
    ```sh
    dotnet run
    ```

