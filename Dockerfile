LABEL maintainer="Jasper Arildslund <jbjar@cbb.dk>"
LABEL repository="https://github.com/mvno/favro-updater"
LABEL homepage="https://github.com/mvno/favro-updater"

LABEL com.github.actions.name="Favro updater"
LABEL com.github.actions.description="A Github action that updates Favro cards"
LABEL com.github.actions.icon="sliders"
LABEL com.github.actions.color="purple"

FROM mcr.microsoft.com/dotnet/runtime:6.0
RUN curl https://github.com/mvno/favro-updater/releases/download/v0.0.4/FavroUpdater.Console.0.0.4.zip -O tmp \
&& unzip tmp/FavroUpdater.Console.0.0.4.zip -d . \
&& rm tmp/FavroUpdater.Console.0.0.4.zip

ENTRYPOINT [ "dotnet", "/FavroUpdater.Console.dll" ]
