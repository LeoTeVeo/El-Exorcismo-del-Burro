# El-Exorcismo-del-Burro (Proyecto de Desarrollo Personal)

¡Hola! Este proyecto es mi primer desarrollo de un videojuego sin ser una entrega universitaria. Nace de la ambición de crear una experiencia de terror psicológico inmersiva 
con estética *lo-fi/PSX*, construida desde cero de forma independiente.

---

## Objetivos del Proyecto

* **Código Robusto y Reutilizable:** En el desarrollo de este proyecto quiero aprender nuevas técnicas para programar en Unity que me sirvan para conseguir un código robusto, limpio y reutilizable. Busco implementar arquitecturas modulares que puedan adaptarse a futuros proyectos.
* **Diseño de Niveles y Pulido:** También quiero mejorar mi diseño de niveles y la pulcritud de las mecánicas base de juego para que la experiencia del jugador sea lo mejor posible, cuidando la navegación, la iluminación y el *game feel*.
* **Crecimiento Personal:** Por último, quiero probarme a mí mismo y mejorar mi constancia y compromiso conmigo mismo y mis ambiciones personales, llevando un proyecto desde la fase de concepto hasta un producto final jugable.

---

## Filosofía de Diseño

He elegido una estética de terror ya que, en mi opinión, es el género que permite generar mayores emociones al público de forma más sencilla (considero que es ideal para un primer proyecto).Sin embargo, me he impuesto a mí mismo **que no haya Screamers (Jumpscares genéricos)**. 

Al programar e idear un proyecto, siempre pienso que el resultado debería ser un producto que yo mismo valoraría positivamente si no fuese mío. Por ello, el miedo en este juego no nace de ruidos estridentes e imágenes repentinas, sino de:
* **La tensión psicológica:** El uso del espacio, el aislamiento en el entorno y el suspense a través del guión.
* **El diseño sonoro:** Voces y efectos ambientales dinámicos que juegan con la percepción del jugador.
* **La incertidumbre:** Mecánicas que alteran el entorno y obligan al jugador a dudar de lo que ve.

---

## Implementación Técnica Destacada (Unity & C#)

Para lograr un flujo de trabajo limpio y eficiente como desarrollador en solitario (*solo dev*), me he enfocado en las siguientes soluciones técnicas:

* **Sistemas de Diálogo Modulares:** Diseño de una estructura de datos personalizada (`struct`) que permite crear guiones dinámicos multipersonaje desde el Inspector de Unity, optimizando el renderizado de texto letra a letra (*typewriter effect*).
* **Audio Espacializado (3D Soundscapes):** Gestión de múltiples `AudioSource` dinámicos asignados por código según el personaje que habla. Esto genera un posicionamiento sonoro tridimensional real (por ejemplo, escuchar a un personaje hablar de forma amortiguada a través de una puerta cerrada).
* **Gestión del Alcance (Scope Management):** Capacidad de análisis crítico para pivotar estrategias de desarrollo; por ejemplo, descartar sistemas o mecánicas de juego complejas y difíciles de implementar con mi nivel actual para priorizar el rendimiento técnico y la atmósfera.
* **Mecánicas Basadas en Estados:** Creación de lógicas secundarias en el entorno (como modos alternativos de juego/percepción) controlados por sistemas de eventos limpios.
* * **Uso de codigos reutilizables e Estratégias de programación:** Prioridad en el diseño de un codigo reutilizable y limpio. Desaprender mañas y practicas poco eficientes o correctas y empezar a usar patrones en los codigos como Command, Observer o Strategy.

---

## Contacto y Enlaces

Si eres un reclutador o desarrollador interesado en los detalles técnicos de este proyecto, no dudes en revisar los scripts del repositorio o contactar conmigo:

* 💼 **Itch.io:** https://leoteveo.itch.io/
* 📧 **Email:** leoneldigiorgi@gmail.com
