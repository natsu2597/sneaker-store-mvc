document.querySelectorAll(".password-toggle").forEach(button => {

    button.addEventListener("click", () => {

        const input = document.getElementById(button.dataset.target);
        const icon = button.querySelector("i");

        if (input.type === "password") {

            input.type = "text";
            icon.classList.replace("bi-emoji-dizzy", "bi-emoji-expressionless");
        }

        else {

            input.type = "password";
            icon.classList.replace("bi-emoji-expressionless", "bi-emoji-dizzy");
        }
    })

    

})