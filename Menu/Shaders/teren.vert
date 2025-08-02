#version 330 core

// vhod
layout(location = 0) in vec3 aPozicija;
layout(location = 1) in vec3 aNormala;
layout(location = 2) in vec2 aKoordinataTeksture;

// izhod
out vec3 vNormala;
out vec3 vPozicija;
out vec4 vTezeTekstur1; // 9 tekstur ampak ni tabel večjih od 4 (lahko bi tudi vsako posebej)
out vec4 vTezeTekstur2;
out float vTezaKamen;

// parametri
uniform mat4 uModel;
uniform mat4 uOgled;
uniform mat4 uProjekcija;
uniform vec2 uIzbiraBarve;


void main()
{
    vNormala = aNormala;
    vec4 pravaPozicija = uModel * vec4(aPozicija, 1.0);
    vPozicija = pravaPozicija.xyz;

    float visina = vPozicija.y;
    float naklon = abs(dot(aNormala, vec3(0.0, 1.0, 0.0)));
    

    vTezeTekstur1 = vec4(0.0);
    vTezeTekstur2 = vec4(0.0);
    vTezaKamen = 0.0;

    float naklonKamna = 0.8;
    float epsilon = 0.1;

    if (visina < uIzbiraBarve.x - 0.25) {
        if (naklon < naklonKamna) {
            vTezeTekstur2.y = 1.0; // kamenPesek
        }
        else{
            vTezeTekstur1.x = 1.0; // pesek
        }
    }
    else if (visina < uIzbiraBarve.x + 0.25) {
        if (naklon < naklonKamna) {
            vTezeTekstur2.z = 1.0; // kamenTrava
        }
        else {
            vTezeTekstur1.w = 1.0; // pesekTrava        
        }
    }
    else if (visina < uIzbiraBarve.y - 2.0) {
        if (naklon < naklonKamna) {
            vTezeTekstur2.z = 1.0; // kamenTrava
        }
        else {
            vTezeTekstur1.y = 1.0; // trava
        }
    }
    else if (visina < uIzbiraBarve.y + 2.0) {
        if (naklon < naklonKamna) {
            vTezeTekstur2.w = 1.0; // kamenSneg
        }
        else {
            vTezeTekstur2.x = 1.0; // travaSneg
        }
    }
    else {
        if (naklon < naklonKamna) {
            vTezeTekstur2.w = 1.0; // kamenSneg
        }
        else {
            vTezeTekstur1.z = 1.0; // sneg 
        }
    }

    if (naklon < (naklonKamna - epsilon)) {
        vTezaKamen = 1.0; // kamen;
        vTezeTekstur1 = vec4(0.0);
        vTezeTekstur2 = vec4(0.0);
    }

    // končna pozicija točke v svetu (ime gl_Position je nujno)
    gl_Position = uProjekcija * uOgled * pravaPozicija;
}

