# **International Business Men**
*(Duración máxima: 4 horas)*

Trabajas para el GNB (Gloiath National Bank), y tu jefe, Antonio Fernandez, te ha pedido que diseñes e implementes una aplicación backend para ayudar a los ejecutivos de la empresa que vuelan por todo el mundo. Los ejecutivos necesitan un listado de cada producto con el que GNB comercia, y el total de la suma de las ventas de estos productos.

Para esta tarea debes crear un webservice. Este webservice devuelve los datos en JSON.

Recursos a utilizar:

- Archivo rates.json;

**JSON**
```
[
 { "from": "EUR", "to": "USD", "rate": "1.359" },
 { "from": "CAD", "to": "EUR", "rate": "0.732" },
 { "from": "USD", "to": "EUR", "rate": "0.736" },
 { "from": "EUR", "to": "CAD", "rate": "1.366" }
]
```

Cada entrada en la colección especifica una conversión de una moneda a otra (cuando te devuelve una conversión, la conversión contraria también se devuelve), sin embargo hay algunas conversiones que no se devuelven, y en caso de ser necesarias, deberán ser calculadas utilizando las conversiones que se dispongan. Por ejemplo, en el ejemplo no se envía la conversión de USD a CAD, esta debe ser calculada usando la conversión USD a EUR y después EUR a CAD.

- Archivo transactions.json:

**JSON**
```
[
 { "sku": "T2006", "amount": "10.00", "currency": "USD" },
 { "sku": "M2007", "amount": "34.57", "currency": "CAD" },
 { "sku": "R2008", "amount": "17.95", "currency": "USD" },
 { "sku": "T2006", "amount": "7.63", "currency": "EUR" },
 { "sku": "B2009", "amount": "21.23", "currency": "USD" }
]
```

Cada entrada en la colección representa una transacción de un producto (el cual se identifica mediante el campo SKU), el valor de dicha transacción (amount) y la moneda utilizada (currency).

La aplicación debe tener un método desde el cuál se pueda obtener el listado de todas las transacciones. Otro método con el que obtener todos los rates. Y por último un método al que se le pase un SKU, y devuelva un listado con todas las transacciones de ese producto en EUR, y la suma total de todas esas transacciones, también en EUR.

Por ejemplo, utilizando los datos anteriores, la suma total para el producto T2006 debería ser 14,99.

## **Requisitos**

    Se requiere la utilizacion de la última version disponible del framework .NET CORE de Microsoft.
    Recuerda que pueden faltar algunas conversiones, deberás calcularlas mediante la información que tengas.
    Separación de responsabilidades en distintas capas: Servicios distribuidos, capa de aplicación, capa de dominio.
    Implementación de log de error y manejo de excepciones en cada capa.
    Tener en cuenta los principios SOLID y correcta capitalización del código.
    Uso de Inyección de dependencias.

## **Puntos extra (No obligatorios)**

    Estamos tratando con divisas, intenta no utilizar números con coma flotante.
    Después de cada conversión, el resultado debe ser redondeado a dos decimales usando el redondeo Banker's Rounding (http://en.wikipedia.org/wiki/Rounding#Round_half_to_even)
    
 **Por favor, el comentario del commit final ha de ser "Finished", para informarnos de que se ha finalizado la prueba.**
