/*

    Diferença entre Eventos ON e addEventListener:

    Eventos ON (onclick, onload, etc.): Só permite definir uma função para um determinado evento (Causa mau funcionamento se dois scripts
                                        diferentes acessarem o evento de um mesmo elemento.).

    addEventListener (click, load, etc.): Permite definir várias funções para um mesmo evento de um elemento.

*/

// Functions:

function ADD_Menu_Events()
{

    // Abrir menu.

    document.getElementById("open-menu").onclick = function() {

        document.getElementById("blocker").style.transform = "translateX(0%)";

    };

    // Fechar menu.

    document.getElementById("close-menu").onclick = function() {

        document.getElementById("blocker").style.transform = "translateX(-100%)";

    };

    window.onclick = function(element) {

        if(element.target === document.getElementById("blocker"))
        {

            document.getElementById("blocker").style.transform = "translateX(-100%)";

        }

    };

}

// Events:

window.addEventListener("load", function() {

    if(document.getElementById("menu") !== null)
    {

        ADD_Menu_Events();
        
    }

});