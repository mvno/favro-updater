FROM mcr.microsoft.com/dotnet/runtime:6.0

LABEL maintainer="Jasper Arildslund <jbjar@cbb.dk>"
LABEL repository="https://github.com/mvno/favro-updater"
LABEL homepage="https://github.com/mvno/favro-updater"

LABEL com.github.actions.name="Favro updater"
LABEL com.github.actions.description="A Github action that updates Favro cards"
LABEL com.github.actions.icon="sliders"
LABEL com.github.actions.color="purple"

COPY https://github.com/mvno/favro-updater/releases/download/v0.0.5/FavroUpdater.Console.0.0.6.zip tmp/
RUN unzip tmp/FavroUpdater.Console.0.0.6.zip -d .
RUN rm tmp/FavroUpdater.Console.0.0.6.zip

ENTRYPOINT [ "dotnet", "/FavroUpdater.Console.dll" ]
