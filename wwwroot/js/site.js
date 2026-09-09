class Base64 {
    static #textEncoder = new TextEncoder();
    static #textDecoder = new TextDecoder();

    // https://datatracker.ietf.org/doc/html/rfc4648#section-4
    static encode = (str) => btoa(String.fromCharCode(...Base64.#textEncoder.encode(str)));
    static decode = (str) => Base64.#textDecoder.decode(Uint8Array.from(atob(str), c => c.charCodeAt(0)));

    // https://datatracker.ietf.org/doc/html/rfc4648#section-5
    static encodeUrl = (str) => this.encode(str).replace(/\+/g, '-').replace(/\//g, '_'); //.replace(/=+$/, '');
    static decodeUrl = (str) => this.decode(str.replace(/\-/g, '+').replace(/\_/g, '/'));
}

document.addEventListener('submit', e => {
    const form = e.target;
    if (form.id == 'auth-form') {
        e.preventDefault();

        const formData = new FormData(form);
        const login = formData.get('auth-login');
        const password = formData.get('auth-password');
        let errorMessage = "";
        if (login.trim().length === 0) {
            errorMessage += "Логин не может быть пустым";
        }
        if (password.trim().length === 0) {
            errorMessage += "Пароль не может быть пустым";
        }
        const err = document.getElementById("auth-modal-error");
        if (errorMessage.length > 0) {
            err.innerText = errorMessage;
            err.style.visibility = "visible"
            return;
        }

        else {
            err.innerText = "";
            err.style.visibility = "hidden";
        }
        console.log(login, password);


        const userPass = login + ':' + password;

        const credentials = Base64.encode(userPass);

        fetch("/User/BasicAuth", {
            headers: {
                "Authorization": "Basic " + credentials,
            }
        }).then(r => {
            if (r.ok) {
                // return r.json();
                // при работе с сессиями при положительном ответе
                // следует перезагркзить страничку.
                // Это должно активировать работу кукки

                window.location.reload();
            }
            else {
                return r.text();
            }
        }).then(console.log);

        console.log(credentials);
    }
    else if (form.id == 'admin-add-group') {
        e.preventDefault();
        const formData = new FormData(form);
        fetch("/Admin/AddGroup", {
            method: "POST",
            body: formData
        }).then(r => {
            /* if (r.ok) */
            {
                r.text().then(alert);
            }
        });
    }
});