/* ============================================================
   SMS — Student Management System | app.js
   Indira University · School of IT · FY MSC-CA 2025-2026
   ============================================================ */

/* ── 1. HEADER scroll shadow ── */
const siteHeader = document.getElementById('site-header');
window.addEventListener('scroll', () => {
    siteHeader.classList.toggle('scrolled', window.scrollY > 20);
    updateScrollTopBtn();
    updateActiveNavLink();
});

/* ── 2. MOBILE NAV ── */
const hamburger = document.getElementById('hamburger');
const mobileNav = document.getElementById('mobile-nav');

hamburger.addEventListener('click', () => {
    mobileNav.classList.toggle('open');
    const icon = hamburger.querySelector('i');
    icon.classList.toggle('fa-bars', !mobileNav.classList.contains('open'));
    icon.classList.toggle('fa-times', mobileNav.classList.contains('open'));
});

document.querySelectorAll('.mob-link').forEach(link => {
    link.addEventListener('click', () => {
        mobileNav.classList.remove('open');
        hamburger.querySelector('i').classList.replace('fa-times', 'fa-bars');
    });
});

/* ── 3. ACTIVE NAV LINK ── */
const sections = ['home', 'about', 'features', 'contact'];
const navLinks = document.querySelectorAll('.nav-link');

function updateActiveNavLink() {
    let current = 'home';
    sections.forEach(id => {
        const el = document.getElementById(id);
        if (el && window.scrollY >= el.offsetTop - 120) current = id;
    });
    navLinks.forEach(link => {
        const href = link.getAttribute('href').replace('#', '');
        link.classList.toggle('active', href === current);
    });
}
updateActiveNavLink();

/* ── 4. REVEAL ON SCROLL ── */
const revealObserver = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.classList.add('visible');
            revealObserver.unobserve(entry.target);
        }
    });
}, { threshold: 0.1 });
document.querySelectorAll('.reveal').forEach(el => revealObserver.observe(el));

/* ── 5. SCROLL TO TOP ── */
const scrollTopBtn = document.getElementById('scroll-top-btn');
function updateScrollTopBtn() {
    scrollTopBtn.classList.toggle('visible', window.scrollY > 300);
}
scrollTopBtn.addEventListener('click', () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
});

/* ══════════════════════════════════════════
   6. LOGIN MODAL
   ══════════════════════════════════════════ */
const loginOverlay = document.getElementById('login-overlay');
const loginCloseBtn = document.getElementById('login-close-btn');
const lmHint = document.getElementById('lm-hint');
const lmError = document.getElementById('lm-error');
const lmUser = document.getElementById('lm-user');
const lmPass = document.getElementById('lm-pass');
const lmSubmitBtn = document.getElementById('lm-submit-btn');

const CREDENTIALS = {
    admin: { user: 'admin', pass: 'admin123' },
    teacher: { user: 'teacher', pass: 'teach123' },
    student: { user: 'student', pass: 'stu123' }
};

const HINTS = {
    admin: 'Demo: admin / admin123',
    teacher: 'Demo: teacher / teach123',
    student: 'Demo: student / stu123'
};

const REDIRECT_URLS = {
    admin: '/Admin/Dashboard',
    teacher: '/Teacher/Dashboard',
    student: '/Student/Dashboard'
};

let currentRole = 'admin';

function openLogin(e) {
    if (e) e.preventDefault();
    loginOverlay.classList.add('open');
    lmError.textContent = '';
    lmUser.value = '';
    lmPass.value = '';
    setTimeout(() => lmUser.focus(), 200);
}

function openLoginWithRole(role) {
    if (event) event.preventDefault();
    currentRole = role;
    syncRoleTabs();
    openLogin();
}

function closeLogin() {
    loginOverlay.classList.remove('open');
    lmError.textContent = '';
}

loginOverlay.addEventListener('click', (e) => { if (e.target === loginOverlay) closeLogin(); });
loginCloseBtn.addEventListener('click', closeLogin);

document.querySelectorAll('.rtab').forEach(tab => {
    tab.addEventListener('click', () => {
        currentRole = tab.dataset.role;
        syncRoleTabs();
        lmError.textContent = '';
    });
});

function syncRoleTabs() {
    document.querySelectorAll('.rtab').forEach(t => {
        t.classList.toggle('active', t.dataset.role === currentRole);
    });
    lmHint.textContent = HINTS[currentRole];
}

lmSubmitBtn.addEventListener('click', doLogin);
lmPass.addEventListener('keydown', (e) => { if (e.key === 'Enter') doLogin(); });

function doLogin() {
    const username = lmUser.value.trim();
    const password = lmPass.value.trim();

    if (!username || !password) {
        lmError.textContent = 'Please enter both username and password.';
        return;
    }

    const expected = CREDENTIALS[currentRole];

    if (username === expected.user && password === expected.pass) {
        lmError.textContent = '';
        lmSubmitBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Signing in…';
        lmSubmitBtn.disabled = true;

        sessionStorage.setItem('sms_role', currentRole);
        sessionStorage.setItem('sms_user', username);

        setTimeout(() => {
            window.location.href = REDIRECT_URLS[currentRole];
        }, 800);

    } else {
        lmError.textContent = 'Invalid username or password.';
        lmPass.value = '';
        lmPass.focus();
        lmSubmitBtn.disabled = false;
    }
}

document.getElementById('header-login-btn').addEventListener('click', openLogin);
document.getElementById('hero-launch-btn').addEventListener('click', openLogin);
document.getElementById('mob-login-btn').addEventListener('click', (e) => {
    mobileNav.classList.remove('open');
    hamburger.querySelector('i').classList.replace('fa-times', 'fa-bars');
    openLogin(e);
});
document.querySelectorAll('.footer-login-link').forEach(link => {
    link.addEventListener('click', (e) => {
        e.preventDefault();
        openLoginWithRole(link.dataset.role);
    });
});

/* ── 7. CONTACT FORM ── */
const contactSubmitBtn = document.getElementById('contact-submit-btn');
contactSubmitBtn.addEventListener('click', (e) => {
    e.preventDefault();
    const original = contactSubmitBtn.innerHTML;
    contactSubmitBtn.innerHTML = '<i class="fas fa-check"></i> Message Sent!';
    contactSubmitBtn.style.background = 'linear-gradient(135deg, #16a34a, #15803d)';
    contactSubmitBtn.disabled = true;
    setTimeout(() => {
        contactSubmitBtn.innerHTML = original;
        contactSubmitBtn.style.background = '';
        contactSubmitBtn.disabled = false;
    }, 3000);
});

/* ══════════════════════════════════════════
   8. SMOOTH SCROLL — FIXED
   Header 70px fixed hai isliye har section
   70px upar se scroll karna padta hai.
   scrollIntoView() yeh consider nahi karta
   isliye manually calculate kar rahe hain.
   ══════════════════════════════════════════ */
const HEADER_HEIGHT = 70; // header ki exact height

document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        const href = this.getAttribute('href');

        // Sirf valid section IDs ke liye kaam karo
        if (!href || href === '#') return;

        const target = document.querySelector(href);
        if (!target) return;

        e.preventDefault();

        // Target ki exact position calculate karo
        // window.scrollY = current scroll position
        // getBoundingClientRect().top = target ka viewport se distance
        // HEADER_HEIGHT = fixed header ki height
        const targetPosition = target.getBoundingClientRect().top
            + window.scrollY
            - HEADER_HEIGHT;

        window.scrollTo({
            top: targetPosition,
            behavior: 'smooth'
        });
    });
});