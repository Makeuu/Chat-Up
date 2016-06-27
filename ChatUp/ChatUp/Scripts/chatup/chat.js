(function () {
    var secPannelClasses = ['l0', 'l2'],
        mainPannelClasses = ['l10', 'l8'];

    var sendbox = document.querySelector("#Message_Message"),
        chat = document.querySelector("#listeMessages");

    var panneauLateral = document.querySelector("#panneauLateral"),
        panneauDiscussion = document.querySelector("#panneauDiscussion"),
        lienpanneauLateral = document.querySelector("#lienpanneauLateral");

    var headerInformation = document.querySelector("#headerInformation"),
        bodyInformation = document.querySelector("#bodyInformation");

    window.onload = function (e) {
        sendbox.value = "";
        sendbox.focus();

        $('.modal-trigger').leanModal({
            dismissible: true, // Modal can be dismissed by clicking outside of the modal
            opacity: 0, // Opacity of modal background
            in_duration: 300, // Transition in duration
            out_duration: 200, // Transition out duration
            ready: function () {}, // Callback for Modal open
            complete: function () {} // Callback for Modal close
        });

        chat.scrollTop = chat.scrollHeight;

        lienpanneauLateral.addEventListener('click', function () {
            for (var i = 0; i < mainPannelClasses.length; i++) {
                panneauDiscussion.classList.toggle(mainPannelClasses[i]);
                panneauLateral.classList.toggle(secPannelClasses[i]);
            }

            panneauLateral.classList.toggle('pannel-visible');

        }, false);

        headerInformation.addEventListener('click', function () {
            bodyInformation.classList.toggle('pannel-visible');
        }, false);

    };
})();