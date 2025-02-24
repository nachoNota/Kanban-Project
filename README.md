# Tableros Kanban: Proyecto final TL2
  Este proyecto consiste en ser un gestor de tableros Kanban, donde cada usuario puede contar con distintos tableros, cada uno con sus tares correspondientes, las cuales, a
  su vez, pueden pertenecer a uno de los cinco estados disponibles (Ideas, ToDo, Doing, Review, Done). Se le proporciona al usuario una interfaz intuitiva, y, en el caso de 
  que suceda algún error inesperado, es redirigido a una vista adecuada.
  
  He utiliza un patrón de diseño MVC (Model-View-Controler), lo cual permite desacoplar la interfaz de usuario (vista), los datos (modelo) y la lógica de la 
  aplicación (controlador), que ayuda a lograr la separación de preocupaciones.
  
  Además, para lograr llevar a cabo el principio de responsabilidad única (uno de los principios S.O.L.I.D), se utiliza un patrón llamado repositorio, con el cual puedo traer
  datos externos a mi aplicación que vaya a necesitar, en este caso, desde una base de datos.
  
  En cuanto a la base de datos, en este proyecto utilicé MySql, para el cual almacené toda la informacion respecto de los usuarios, tableros y tareas.

## 🛠️ Funcionamiento del sistema
- **Acceso al sistema:** Los usuarios que tengan una cuenta registrada dentro de la aplicación son los únicos que pueden acceder al sistema, caso contrario no se puede
interactuar con ninguna funcionalidad.
- **Gestión de usuarios:** Un usuario no logueado dentro del sistema puede crearse su propio usuario, el cual tendrá por defecto el rol de operador. El admin también puede
crear usuarios, y asignarles alguno de los roles disponibles.
- **Asignación de tareas:** Dentro de tu propio tablero, puedes asignarle una tarea en particular a cualquier usuario del sistema, solo tienes que buscar su nombre y
asignarsela.
- **Visualización de tableros:** si tienes el rol de operador, puedes ver tus propios tableros y tableros ajenos en los que tengas tareas asignadas. Por otra parte, si eres
admin, se agrega una nueva opción para buscar tableros de un usuario en específico, sin importar que no tengas tareas asignadas en ese tablero.

## 📂 Distribución de carpetas
- /Controllers --> Contiene los controladores, manejando la lógica de aplicacion y procesando solicitudes de usuarios.
- /Database --> Contiene clases responsables de la gestión de la base de datos, incluyendo la conexión y la creación de comandos.

  1. MySqlConnectionProvider: Administra la conexión a la base de datos recibiendo por constructor la cadena de conexión.
  2. MySqlCommandFactory: Crea comandos SQL para ejecutar consultas en la base de datos.

  La decisión de hacer esta carpeta fue que siempre existe la posibilidad de querer cambiar la base de datos con la que uno trabaja, por lo que, en vez de tener que cambiar
  cada uno de los metodos de todos los repositorios, directamente cambio estas dos clases, y lo demás sigue funcionando.
  
- /Filters --> Contiene el manejo necesario para limitar funcionalidades del sistema dependiendo del rol del usuario dentro de este.

  1. AccessLevelAttribute: Esta clase lo que permite es crear un atributo llamado AccessLevel que puede ser utilizado en los metodos o clases de los controladores, con el fin
  de poder filtar el acceso al sistema entre los que tienen rol de operador y los administradores. 
  
- /Models --> Contiene los datos que representan las entidades que se utilizan en el sistema.

  1. Usuario, que cuenta con id, nombre, contraseña, email y rol
  2. Tarea, que cuenta con id, id del propietario, id del tablero al que pertenece, estado, titulo, descripción, color y la fecha en la que fue modificada por última vez.
  3. Tablero, que cuenta con id, id del propietario, titulo, color y descripción.
   
- /Repositories --> Utilizado para traer datos externos a la aplicación. Cada modelo tiene su propio repositorio.
- /Services --> Contiene servicios de autenticacion, donde, en caso de un login exitoso, se guardan los datos del usuario en variables de sesion.

  1. AuthenticationService: es el responsable de gestionar el inicio de sesión junto a las variables de sesión correspondientes. Es una implementación de
  IAuthenticacionService.
  2. PasswordService: es un servicio que lo utilizo para manejar lo relacionado a las contraseñas hasheadas. Tiene metodos para hashear una contraseña y para verificar si dos
  contraseñas (una hasheada y la otra no) coinciden. 

- /ViewModels --> Contiene los objetos que son pasados a las vistas para facilitar la presentación de los datos.
- /Views --> Contiene todo lo relacionado con la interfaz de usuario.

## ✨ Uso
Para poder utilizar la base de datos correspondiente, necesitas guardar el archivo .sql que se encuentra en la carpeta Database. Luego, en Mysql Workbench, ve a tu localhost, apreta la
opcion server, y luego data import. Por último, debes cambiar la cadena de conexión en appsettings.json con tus respectivos datos.

USUARIOS DE PRUEBA:

 1. USUARIO: elAdmin | CONTRASEÑA: 123456 | ROL: Admin
 2. USUARIO: pruebas123 | CONTRASEÑA: pruebas123 | ROL: Operador

- En primera instancia, vas a toparte con dos vistas, una para loguearse, y otra para registrarte si es que todavía no tienes una cuenta:
 <div style="display: flex; flex-wrap: wrap;">
    <img src="https://github.com/user-attachments/assets/477fa78b-87b7-47ec-be76-9e5b9074e0e3" style="margin: 5px;" width="400">
    <img src="https://github.com/user-attachments/assets/8913657d-1a8b-468f-aa73-440266f8b637" style="margin: 5px;" width="400">
</div>

- Luego, una vez iniciada la sesión, vas a poder visualizar los tableros que haz creado, junto con las opciones para crearlos, modificarlos o eliminarlos: 

<img src="https://github.com/user-attachments/assets/d412916e-d88e-44ec-b71c-4c84edaca1f9" width="400">

- En cuanto al usuario, puedes entrar a los ajustes desde la parte superior derecha, y encontrarás distintas opciones, dependiendo del
rol que tenga tu usuario (la siguiente vista es la del admin, un operador tendría solo las primeras tres opciones):

<img src="https://github.com/user-attachments/assets/e409b7ea-4da4-4d53-9afb-6d1fafe5ec7c" style="margin: 5px;" width="300" height="250">

- Volviendo a los tableros, tenemos la opción de poder ver todos los tableros en los que podamos tener alguna tarea asignada dentro:

<img src="https://github.com/user-attachments/assets/26ac1e2c-9475-4b68-8820-71e72f5f2c80" width="400">

- Además, si tienes un rol de administrador, tienes acceso a todos los tableros dentro del sistema, pero no puedes verlos todos al mismo
tiempo, debes filtarlos por el nombre del usuario del que quieras visualizar los tableros.

- Esto es lo que verías cuando entras a uno de los tableros, una serie de tablas con sus correspondientes tareas. Ves que algunas tareas
tienen un icono de un usuario y otras no? si la tiene, significa que vos sos el propietario de esa tarea, caso contrario otra persona es
el propietario (esto se ve pasando el mouse por encima del icono).

<img src="https://github.com/user-attachments/assets/8e956205-9a80-4205-8971-9343f7554a8a" width="400">

- Por último, puedes visualizar los detalles de las tareas haciendo click en alguna de ellas. La siguiente vista es lo que verías si eres
el propietario del tablero, además de ser el propietario de la tarea. Si alguno de estos factores varía, tendrás las acciones más limitadas.

<img src="https://github.com/user-attachments/assets/558d0731-5f51-43cc-84eb-71b534915004" width="400">

 ## 👤 Autor 
 Nota, José Ignacio
 

