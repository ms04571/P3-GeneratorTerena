#version 400 core

// vhod
layout(location = 0) in vec3 aPozicija;
layout(location = 1) in vec3 aNormala;

// izhod
out vec3 vNormala;
out vec3 vPozicija;
out vec2 vKoordinataTeksture;
out vec4 vTezeTekstur1;  // 9 tekstur ampak  
out vec4 vTezeTekstur2;  // ni tabel večjih od 4
out float vTezaKamen;    // (lahko bi tudi vsako posebej)

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
    vKoordinataTeksture = pravaPozicija.xz * 0.05;

    float visina = vPozicija.y;
    float naklon = abs(dot(aNormala, vec3(0.0, 1.0, 0.0)));
    

    vTezeTekstur1 = vec4(0.0);
    vTezeTekstur2 = vec4(0.0);
    vTezaKamen = 0.0;

    // koliko mešane teksture bo
    float pesekTravaSirina = 0.25;
    float travaSnegSirina = 2;

    float naklonKamna = 0.8;
    float epsilon = 0.1;

    if (visina < uIzbiraBarve.x - pesekTravaSirina) {
        if (naklon < naklonKamna) {
            vTezeTekstur2.y = 1.0; // kamenPesek
        }
        else{
            vTezeTekstur1.x = 1.0; // pesek
        }
    }
    else if (visina < uIzbiraBarve.x + pesekTravaSirina) {
        if (naklon < naklonKamna) {
            vTezeTekstur2.z = 1.0; // kamenTrava
        }
        else {
            vTezeTekstur1.w = 1.0; // pesekTrava        
        }
    }
    else if (visina < uIzbiraBarve.y - travaSnegSirina) {
        if (naklon < naklonKamna) {
            vTezeTekstur2.z = 1.0; // kamenTrava
        }
        else {
            vTezeTekstur1.y = 1.0; // trava
        }
    }
    else if (visina < uIzbiraBarve.y + travaSnegSirina) {
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

