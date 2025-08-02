#version 330 core

//vhod
in vec3 vNormala;
in vec3 vPozicija;
flat in float vIdTeksture;

// izhod
out vec4 BarvaPiksla;


// PARAMETRI
in vec4 vTezeTekstur1;
in vec4 vTezeTekstur2;
in float vTezaKamen;

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

vec4 Triplanar(sampler2D tekstura, vec3 normala, vec3 pozicija)
{
    vec3 mesanica = abs(normala);
    mesanica = pow(mesanica, vec3(4.0));
    mesanica /= (mesanica.x + mesanica.y + mesanica.z);

    vec2 koordXY = pozicija.xy * 0.05;
    vec2 koordYZ = pozicija.yz * 0.05;
    vec2 koordXZ = pozicija.xz * 0.05;

    vec4 teksXY = texture(tekstura, koordXY);
    vec4 teksYZ = texture(tekstura, koordYZ);
    vec4 teksXZ = texture(tekstura, koordXZ);

    return teksXY * mesanica.z + teksYZ * mesanica.x + teksXZ * mesanica.y;
}

void main()
{
    vec4 barva = vec4(0.0);

    barva += Triplanar(uPesek,       vNormala, vPozicija) * vTezeTekstur1.x;
    barva += Triplanar(uTrava,       vNormala, vPozicija) * vTezeTekstur1.y;
    barva += Triplanar(uSneg,        vNormala, vPozicija) * vTezeTekstur1.z;
    barva += Triplanar(uPesekTrava,  vNormala, vPozicija) * vTezeTekstur1.w;

    barva += Triplanar(uTravaSneg,   vNormala, vPozicija) * vTezeTekstur2.x;
    barva += Triplanar(uKamenPesek,  vNormala, vPozicija) * vTezeTekstur2.y;
    barva += Triplanar(uKamenTrava,  vNormala, vPozicija) * vTezeTekstur2.z;
    barva += Triplanar(uKamenSneg,   vNormala, vPozicija) * vTezeTekstur2.w;

    barva += Triplanar(uKamen,       vNormala, vPozicija) * vTezaKamen;

    // Osvetljava
    float diffuse = max(dot(vNormala, uSmerSvetlobe), 0.3);
    BarvaPiksla = vec4(barva.rgb * diffuse, 1.0);
}

