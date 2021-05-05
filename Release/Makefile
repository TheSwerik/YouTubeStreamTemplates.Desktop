OPTDIR = $(DESTDIR)/opt
NAME = YouTubeStreamTemplates

install:
		@echo $(NAME)
		find . -type f ! -path "./Makefile" -exec install -Dm 755 "{}" "$(OPTDIR)/$(NAME)/{}" \;
		export PATH=$$PATH:/opt/$(NAME)
		@if [ -d "/usr/share/applications/" ]; then\
			echo "[Desktop Entry]\nName=$(NAME)\nExec=/opt/$(NAME)/YouTubeStreamTemplatesCrossPlatform\nTerminal=false\nType=Application" > /usr/share/applications/$(NAME).desktop;\
		fi
		@echo "Icon=/opt/$(APP)/icon"

uninstall:
		rm $(OPTDIR)/$(NAME)
		@if [ -d "/usr/share/applications/" ]; then\
			rm -r /usr/share/applications/$(NAME).desktop;\
		fi
		@echo "REMOVE PATH"