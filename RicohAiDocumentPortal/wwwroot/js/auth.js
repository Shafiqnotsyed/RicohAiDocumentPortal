import { initializeApp } from "https://www.gstatic.com/firebasejs/11.3.1/firebase-app.js";
import {
    getAuth,
    GoogleAuthProvider,
    GithubAuthProvider,
    signInWithPopup,
    createUserWithEmailAndPassword,
    signInWithEmailAndPassword,
    sendPasswordResetEmail,
    signOut
} from "https://www.gstatic.com/firebasejs/11.3.1/firebase-auth.js";

const container = document.querySelector('.container');
const loginLink = document.querySelector('.SignInLink');
const registerLink = document.querySelector('.SignUpLink');

if (registerLink) {
    registerLink.addEventListener('click', (e) => {
        e.preventDefault();
        container?.classList.add('active');
    });
}

if (loginLink) {
    loginLink.addEventListener('click', (e) => {
        e.preventDefault();
        container?.classList.remove('active');
    });
}

function togglePassword(toggleId, inputId) {
    const toggle = document.getElementById(toggleId);
    const input = document.getElementById(inputId);

    if (!toggle || !input) return;

    toggle.addEventListener('click', function () {
        if (input.type === 'password') {
            input.type = 'text';
            this.setAttribute('name', 'lock-open-alt');
        } else {
            input.type = 'password';
            this.setAttribute('name', 'lock-alt');
        }
    });
}

togglePassword('togglePassword', 'pass1');
togglePassword('togglePassword2', 'pass2');
togglePassword('togglePassword3', 'pass3');

const forgotLink = document.getElementById('forgotLink');
const forgotOverlay = document.getElementById('forgotOverlay');
const forgotClose = document.getElementById('forgotClose');
const sendOtpBtn = document.getElementById('sendOtpBtn');
const okBtn = document.getElementById('okBtn');
const step1 = document.getElementById('step1');
const step2 = document.getElementById('step2');

forgotLink?.addEventListener('click', (e) => {
    e.preventDefault();
    if (step1) step1.style.display = 'block';
    if (step2) step2.style.display = 'none';
    forgotOverlay?.classList.add('active');
});

forgotClose?.addEventListener('click', () => {
    forgotOverlay?.classList.remove('active');
});

forgotOverlay?.addEventListener('click', function (e) {
    if (e.target === this) {
        this.classList.remove('active');
    }
});

const firebaseConfig = {
    apiKey: "AIzaSyCFay_u390Jvxxb5fnLgolIC-o98Tr5U0s",
    authDomain: "webapplication2-c4aa6.firebaseapp.com",
    projectId: "webapplication2-c4aa6",
    storageBucket: "webapplication2-c4aa6.firebasestorage.app",
    messagingSenderId: "621050499658",
    appId: "1:621050499658:web:f7e5f2d4c439565b030cb3",
    measurementId: "G-LB37R11R69"
};

const app = initializeApp(firebaseConfig);
const auth = getAuth(app);

async function setServerSession(email) {
    const tokenField = document.querySelector('input[name="__RequestVerificationToken"]');

    const response = await fetch('/Account/Login?handler=Session', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            ...(tokenField ? { 'RequestVerificationToken': tokenField.value } : {})
        },
        body: JSON.stringify({ email })
    });

    const result = await response.json().catch(() => null);

    if (!response.ok || !result?.success) {
        throw new Error(result?.message || 'Unable to create server session.');
    }

    return result;
}

function redirectAfterLogin(result) {
    const target = result?.redirectUrl || '/Home';
    window.location.href = target;
}

sendOtpBtn?.addEventListener('click', async function () {
    const email = document.getElementById('Emailspace')?.value?.trim();

    if (!email) {
        alert('Please enter your email address.');
        return;
    }

    try {
        await sendPasswordResetEmail(auth, email);

        if (step1) step1.style.display = 'none';
        if (step2) step2.style.display = 'block';
    } catch (error) {
        alert('Error: ' + (error.message || 'Unable to send password reset email.'));
    }
});

okBtn?.addEventListener('click', function () {
    forgotOverlay?.classList.remove('active');

    if (step1) step1.style.display = 'block';
    if (step2) step2.style.display = 'none';
});

document.getElementById('register-btn')?.addEventListener('click', async function (event) {
    event.preventDefault();

    const email = document.getElementById('email2')?.value?.trim();
    const password = document.getElementById('pass2')?.value || '';
    const confirmPassword = document.getElementById('pass3')?.value || '';

    if (!email) {
        alert('Please enter your email address.');
        return;
    }

    const errors = [];

    if (password.length < 6) errors.push('• Minimum of 6 characters');
    if (!/[A-Z]/.test(password)) errors.push('• At least one uppercase letter');
    if (!/[a-z]/.test(password)) errors.push('• At least one lowercase letter');
    if (!/[0-9]/.test(password)) errors.push('• At least one number');
    if (!/[\^$*.\[\]{}()?"!@#%&/\\,><':;|_~`+\-=]/.test(password)) errors.push('• At least one special character');

    if (errors.length > 0) {
        alert('Password must have:\n' + errors.join('\n'));
        return;
    }

    if (password !== confirmPassword) {
        alert('Passwords do not match.');
        return;
    }

    try {
        const userCredential = await createUserWithEmailAndPassword(auth, email, password);
        const result = await setServerSession(userCredential.user.email || email);
        redirectAfterLogin(result);
    } catch (error) {
        if (error.code === 'auth/email-already-in-use') {
            alert('An account with this email already exists.');
        } else {
            alert(error.message || 'Registration failed.');
        }
    }
});

document.getElementById('login-btn')?.addEventListener('click', async function (event) {
    event.preventDefault();

    const email = document.getElementById('email1')?.value?.trim();
    const password = document.getElementById('pass1')?.value || '';

    if (!email || !password) {
        alert('Please enter both email and password.');
        return;
    }

    try {
        const userCredential = await signInWithEmailAndPassword(auth, email, password);
        const result = await setServerSession(userCredential.user.email || email);
        redirectAfterLogin(result);
    } catch (error) {
        alert('Login failed: ' + (error.message || 'Unable to sign in.'));
    }
});

const googleProvider = new GoogleAuthProvider();

document.getElementById('google-btn')?.addEventListener('click', async function () {
    try {
        const result = await signInWithPopup(auth, googleProvider);
        const sessionResult = await setServerSession(result.user.email || 'google-user');
        redirectAfterLogin(sessionResult);
    } catch (error) {
        alert('Google login failed: ' + (error.message || 'Unable to sign in with Google.'));
    }
});

const githubProvider = new GithubAuthProvider();

document.getElementById('github-btn')?.addEventListener('click', async function () {
    try {
        const result = await signInWithPopup(auth, githubProvider);
        const sessionResult = await setServerSession(result.user.email || 'github-user');
        redirectAfterLogin(sessionResult);
    } catch (error) {
        alert('Github login failed: ' + (error.message || 'Unable to sign in with GitHub.'));
    }
});

window.ricohFirebaseLogout = async function () {
    try {
        await fetch('/Account/Login?handler=Logout', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        });
    } catch (_) {
    }

    try {
        await signOut(auth);
    } catch (_) {
    }

    window.location.href = '/Account/Login';
};