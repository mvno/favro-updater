FROM mcr.microsoft.com/dotnet/sdk:6.0 as build-env

COPY . ./
RUN dotnet publish -c Release -o out --no-self-contained

LABEL maintainer="Jasper Arildslund <jbjar@cbb.dk>"
LABEL repository="https://github.com/mvno/favro-updater"
LABEL homepage="https://github.com/mvno/favro-updater"

LABEL com.github.actions.name="Favro updater"
LABEL com.github.actions.description="A Github action that updates Favro cards"
LABEL com.github.actions.icon="sliders"
LABEL com.github.actions.color="purple"

FROM mcr.microsoft.com/dotnet/runtime:6.0
COPY --from=build-env /out .
ENTRYPOINT [ "dotnet", "/FavroUpdater.Console.dll" ]
