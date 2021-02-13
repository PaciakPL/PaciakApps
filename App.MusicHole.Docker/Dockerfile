FROM centos:centos8 AS build
USER root

RUN ln -sf /usr/share/zoneinfo/Europe/Warsaw /etc/localtime

RUN dnf install -y dotnet-sdk-5.0

RUN mkdir -p /tmp/App.MusicHole.Build
WORKDIR /tmp/build

COPY . .

RUN dotnet restore --runtime linux-x64
RUN dotnet clean --configuration Release
RUN dotnet build --runtime linux-x64 --configuration Release --no-restore App.MusicHole
RUN dotnet publish --runtime linux-x64 --self-contained false --no-restore --output /tmp/publish

FROM centos:centos8 AS final
USER root

RUN ln -sf /usr/share/zoneinfo/Europe/Warsaw /etc/localtime
RUN dnf install -y dotnet-runtime-5.0 passwd

RUN useradd -d /App.MusicHole -s /bin/false -U musichole
RUN passwd -d musichole

WORKDIR /opt/musichole

COPY --from=build /tmp/publish .

VOLUME /opt/musichole/logs

RUN chown -R musichole:musichole /opt/musichole

USER musichole:musichole

ENTRYPOINT (cd /opt/musichole && dotnet App.MusicHole.dll)