# Descripción
El proyecto que trabaje este desafio, es una solución que tiene 2 proyectos dentro. Una es donde esta la api que es de tipo "ASP.NET CORE WEB API" y el segundo proyecto es "Proyecto de pruebas xUnit". Ambos proyectos se trabajó con la version .NET 7.

1. Para el cache use Redis, porque me parecio la mas adecuada para este tipo de proyecto. Para que funcione Redis, se tiene que tener instalado docker. Descargar de https://www.docker.com/products/docker-desktop/
2. Para la creación y consumo de una api externa, use la página recomendada: https://mockapi.io/
3. Para la creación de Logs, use "Serilog"
4. estructure el proyecto en 6 capas (Controller, Service, Dto, Entity, Repository y Utilities), lo podia hacer con menos capas, pero para que sea mas odernado y entendible lo hize asi.
5. Existe una carpeta Logs, que se agrego en automatico cuando se usa Serilog.
6. El proyecto aplica los principios de SOLID y CLEAN CODE.
7. El segundo proyecto de la solución, aplica TDD y pruebas unitarias con XUNIT.
8. Las respuestas de los endpoints avisan al cliente si fue satisfactorio o erroneo su petición.
9. La persistencia de datos se creo una lista estática para la simplicidad de los datos, dentro de la capa REPOSITORY.
10. Si tienen alguna duda o consulta, se pueden comunicar conmigo a mi correo personal : johanroot19@gmail.com
# IMPORTANTE!!!!
 Para este caso solo crear productos con el id del 1 al 10, asi esta modificado la api externa para que pueda retornar un descuento, de lo contrario el descuento sera de 0.
Pero si desean modificar ese servicio y agrandar el límite de ID Product, se puede hacer en:  https://6590d4e28cbbf8afa96bbca3.mockapi.io/GetDiscountById

# Pasos para levantar el proyecto y ejecutarlo correctamente

 1. Descargar el proyecto de github con el link https://github.com/jFloresOrtiz/Tekton-Challenge-Repository
 2. Tener instalado docker y levantar con el puerto 6379, $ docker run -p 6379:6379 redis
 3. Si desean que sea en otro puerto, configurar el puerto del proyecto en "appsettings.json"
