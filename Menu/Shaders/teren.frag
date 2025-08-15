#version 330 core

//vhod
in vec3 vNormala;
in vec3 vPozicija;
in vec2 vKoordinataTeksture;
in vec4 vTezeTekstur1;
in vec4 vTezeTekstur2;
in float vTezaKamen;

//vhod
out vec4 BarvaPiksla;

// teksture
uniform sampler2D uTrava;
uniform sampler2D uPesek;
uniform sampler2D uSneg;
uniform sampler2D uKamen;
uniform sampler2D uTravaSneg;
uniform sampler2D uKamenTrava;
uniform sampler2D uKamenPesek;
uniform sampler2D uKamenSneg;
uniform sampler2D uPesekTrava;

// za osvetljavo
uniform vec3 uSmerSvetlobe;

void main()
{
    vec4 barva = vec4(0.0);

    barva += texture(uPesek,      vKoordinataTeksture) * vTezeTekstur1.x;
    barva += texture(uTrava,      vKoordinataTeksture) * vTezeTekstur1.y;
    barva += texture(uSneg,       vKoordinataTeksture) * vTezeTekstur1.z;
    barva += texture(uPesekTrava, vKoordinataTeksture) * vTezeTekstur1.w;
    barva += texture(uTravaSneg,  vKoordinataTeksture) * vTezeTekstur2.x;
    barva += texture(uKamenPesek, vKoordinataTeksture) * vTezeTekstur2.y;
    barva += texture(uKamenTrava, vKoordinataTeksture) * vTezeTekstur2.z;
    barva += texture(uKamenSneg,  vKoordinataTeksture) * vTezeTekstur2.w;
    barva += texture(uKamen,      vKoordinataTeksture) * vTezaKamen;

    // Osvetljava 
    float diffuse = max(dot(vNormala, uSmerSvetlobe), 0.3);
    BarvaPiksla = vec4(barva.rgb * diffuse, 1.0);
}