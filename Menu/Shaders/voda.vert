#version 400 core
layout (location = 0) in vec3 aPozicija;

uniform mat4 uModel;
uniform mat4 uOgled;
uniform mat4 uProjekcija;

void main()
{
    gl_Position = uProjekcija * uOgled * uModel * vec4(aPozicija, 1.0);
}
