package com.example.java_gui;

import javafx.animation.PauseTransition;
import javafx.embed.swing.SwingFXUtils;
import javafx.fxml.FXML;
import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.scene.control.ComboBox;
import javafx.scene.control.Label;
import javafx.scene.control.TextField;
import javafx.scene.control.TextFormatter;
import javafx.scene.image.Image;
import javafx.scene.image.ImageView;
import javafx.scene.image.PixelReader;
import javafx.scene.image.PixelWriter;
import javafx.scene.image.WritableImage;
import javafx.scene.layout.HBox;
import javafx.scene.layout.VBox;
import javafx.scene.paint.Color;
import javafx.stage.FileChooser;
import javafx.stage.Modality;
import javafx.stage.Popup;
import javafx.stage.Stage;
import javafx.geometry.Insets;
import javafx.geometry.Pos;
import javafx.util.Duration;

import javax.imageio.ImageIO;
import java.awt.geom.AffineTransform;
import java.awt.image.AffineTransformOp;
import java.awt.image.BufferedImage;
import java.io.File;
import java.util.function.UnaryOperator;

public class Start_Screen {
    @FXML
    private ComboBox<String> operacje;
    @FXML
    private ImageView org;
    @FXML
    private Button zapisz_przycisk;
    @FXML
    private ImageView mod;
    @FXML
    private Button wykonaj_przycisk;
    @FXML
    private Button skaluj_przycisk;
    @FXML
    private Button obrotLewo_przycisk;
    @FXML
    private Button obrotPrawo_przycisk;

    @FXML
    private Button Import;

    @FXML
    void wybierzPlik() {
        FileChooser fc = new FileChooser();
        fc.setTitle("Wybierz obraz");
        fc.getExtensionFilters().addAll(
                        new FileChooser.ExtensionFilter("Pliki graficzne",  "*.jpg")
        );
        Stage ownerStage = (Stage) Import.getScene().getWindow();
        File selectedFile = fc.showOpenDialog(ownerStage);

        if (selectedFile != null) {
            Image image = new Image(selectedFile.toURI().toString());
            org.setImage(image);
            mod.setImage(null);
            showToast("Pomyślnie załadowano plik");
            wykonaj_przycisk.disableProperty().set(false);
            zapisz_przycisk.disableProperty().set(false);
            skaluj_przycisk.disableProperty().set(false);
            obrotLewo_przycisk.disableProperty().set(false);
            obrotPrawo_przycisk.disableProperty().set(false);
        }else{
            showToast("Nie udało się załadować pliku");
        }
    }

    @FXML
    void obrocLewo() {
        obrocObraz(-90);
    }

    @FXML
    void obrocPrawo() {
        obrocObraz(90);
    }

    private void obrocObraz(double angle) {
        javafx.scene.image.Image fxImage = mod.getImage() != null ? mod.getImage() : org.getImage();
        if (fxImage == null) return;

        BufferedImage image = SwingFXUtils.fromFXImage(fxImage, null);

        final double rads = Math.toRadians(angle);
        final double sin = Math.abs(Math.sin(rads));
        final double cos = Math.abs(Math.cos(rads));

        final int w = (int) Math.floor(image.getWidth() * cos + image.getHeight() * sin);
        final int h = (int) Math.floor(image.getHeight() * cos + image.getWidth() * sin);

        final BufferedImage rotatedImage = new BufferedImage(w, h, BufferedImage.TYPE_INT_ARGB);

        final AffineTransform at = new AffineTransform();
        at.translate(w / 2.0, h / 2.0);
        at.rotate(rads, 0, 0);
        at.translate(-image.getWidth() / 2.0, -image.getHeight() / 2.0);

        final AffineTransformOp rotateOp = new AffineTransformOp(at, AffineTransformOp.TYPE_BILINEAR);
        rotateOp.filter(image, rotatedImage);

        javafx.scene.image.Image newFxImage = SwingFXUtils.toFXImage(rotatedImage, null);

        mod.setImage(newFxImage);
    }

    @FXML
    void Wykonaj(){
        String operacja = operacje.getValue();

        if (operacja == null) {
            showToast("Nie wybrano operacji do wykonania");
            return;
        }

        if ("Negatyw".equals(operacja)) {
            wykonajNegatyw();
        } else if ("Progowanie".equals(operacja)) {
            progowanieModal();
        } else if ("Konturowanie".equals(operacja)) {
            wykonajWykrywanieKrawędzi();
        } else {
            System.out.println("Uruchamiam operację: " + operacja);
        }
    }

    private void wykonajWykrywanieKrawędzi() {
        Image baseImage = mod.getImage() != null ? mod.getImage() : org.getImage();
        if (baseImage == null) return;

        try {
            int width = (int) baseImage.getWidth();
            int height = (int) baseImage.getHeight();
            WritableImage edgeImage = new WritableImage(width, height);
            PixelReader pixelReader = baseImage.getPixelReader();
            PixelWriter pixelWriter = edgeImage.getPixelWriter();

            int[][] filter = {
                {-1,  -1, -1},
                {-1, 8, -1},
                {-1,  -1, -1}
            };

            for (int y = 1; y < height - 1; y++) {
                for (int x = 1; x < width - 1; x++) {
                    double red = 0.0;
                    double green = 0.0;
                    double blue = 0.0;

                    for (int filterY = -1; filterY <= 1; filterY++) {
                        for (int filterX = -1; filterX <= 1; filterX++) {
                            Color color = pixelReader.getColor(x + filterX, y + filterY);
                            int weight = filter[filterY + 1][filterX + 1];
                            red += color.getRed() * weight;
                            green += color.getGreen() * weight;
                            blue += color.getBlue() * weight;
                        }
                    }
                    red = Math.min(Math.max(Math.abs(red), 0.0), 1.0);
                    green = Math.min(Math.max(Math.abs(green), 0.0), 1.0);
                    blue = Math.min(Math.max(Math.abs(blue), 0.0), 1.0);

                    pixelWriter.setColor(x, y, Color.color(red, green, blue));
                }
            }

            mod.setImage(edgeImage);
            showToast("Konturowanie zostało przeprowadzone pomyślnie!");
        } catch (Exception e) {
            showToast("Nie udało się wykonać konturowania.");
        }
    }

    private void progowanieModal() {
        Stage modalStage = new Stage();
        modalStage.initOwner(operacje.getScene().getWindow());
        modalStage.initModality(Modality.APPLICATION_MODAL);
        modalStage.setTitle("Progowanie");

        VBox vbox = new VBox(15);
        vbox.setPadding(new Insets(20));
        vbox.setAlignment(Pos.CENTER_LEFT);

        Label label = new Label("Wartość progu (0-255):");
        TextField thresholdField = new TextField();
        thresholdField.setTextFormatter(new TextFormatter<>(change -> {
            String text = change.getControlNewText();
            if (text.matches("\\d*")) {
                if (text.isEmpty()) return change;
                int value = Integer.parseInt(text);
                if (value >= 0 && value <= 255) {
                    return change;
                }
            }
            return null;
        }));

        Button executeBtn = new Button("Wykonaj progowanie");
        Button cancelBtn = new Button("Anuluj");

        executeBtn.setOnAction(e -> {
            if (!thresholdField.getText().isEmpty()) {
                int threshold = Integer.parseInt(thresholdField.getText());
                wykonajProgowanie(threshold);
                modalStage.hide();
            }
        });

        cancelBtn.setOnAction(e -> modalStage.hide());

        HBox buttonBox = new HBox(10, executeBtn, cancelBtn);
        buttonBox.setAlignment(Pos.CENTER);
        vbox.getChildren().addAll(label, thresholdField, buttonBox);

        modalStage.setScene(new Scene(vbox));
        modalStage.show();
    }

    private void wykonajProgowanie(int threshold) {
        Image baseImage = mod.getImage() != null ? mod.getImage() : org.getImage();
        if (baseImage == null) return;

        try {
            int width = (int) baseImage.getWidth();
            int height = (int) baseImage.getHeight();
            WritableImage thresholdImage = new WritableImage(width, height);
            PixelReader pixelReader = baseImage.getPixelReader();
            PixelWriter pixelWriter = thresholdImage.getPixelWriter();
            double thresholdValue = threshold / 255.0;

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    Color color = pixelReader.getColor(x, y);
                    double brightness = color.getBrightness();
                    if (brightness < thresholdValue) {
                        pixelWriter.setColor(x, y, Color.BLACK);
                    } else {
                        pixelWriter.setColor(x, y, Color.WHITE);
                    }
                }
            }
            mod.setImage(thresholdImage);
            showToast("Progowanie zostało przeprowadzone pomyślnie!");
        } catch (Exception e) {
            showToast("Nie udało się wykonać progowania.");
        }
    }

    private void wykonajNegatyw() {
        Image baseImage = mod.getImage() != null ? mod.getImage() : org.getImage();
        if (baseImage == null) return;

        try {
            int width = (int) baseImage.getWidth();
            int height = (int) baseImage.getHeight();
            WritableImage negativeImage = new WritableImage(width, height);
            PixelReader pixelReader = baseImage.getPixelReader();
            PixelWriter pixelWriter = negativeImage.getPixelWriter();

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    Color color = pixelReader.getColor(x, y);
                    Color negativeColor = Color.color(
                        1.0 - color.getRed(),
                        1.0 - color.getGreen(),
                        1.0 - color.getBlue(),
                        color.getOpacity()
                    );
                    pixelWriter.setColor(x, y, negativeColor);
                }
            }

            mod.setImage(negativeImage);
            showToast("Negatyw został wygenerowany pomyślnie!");
        } catch (Exception e) {
            showToast("Nie udało się wykonać negatywu.");
        }
    }

    @FXML
    void skalujObraz() {
        Stage modalStage = new Stage();
        modalStage.initOwner(operacje.getScene().getWindow());
        modalStage.initModality(Modality.APPLICATION_MODAL);
        modalStage.setTitle("Skalowanie Obrazu");

        VBox vbox = new VBox(15);
        vbox.setPadding(new Insets(20));
        vbox.setAlignment(Pos.CENTER);

        HBox widthBox = new HBox(10);
        widthBox.setAlignment(Pos.CENTER_LEFT);
        Label widthLabel = new Label("Szerokość:");
        TextField widthField = new TextField();
        Label widthError = new Label();
        widthError.setTextFill(Color.RED);
        widthBox.getChildren().addAll(widthLabel, widthField);

        HBox heightBox = new HBox(10);
        heightBox.setAlignment(Pos.CENTER_LEFT);
        Label heightLabel = new Label("Wysokość:");
        TextField heightField = new TextField();
        Label heightError = new Label();
        heightError.setTextFill(Color.RED);
        heightBox.getChildren().addAll(heightLabel, heightField);

        UnaryOperator<TextFormatter.Change> filter = change -> {
            String text = change.getControlNewText();
            if (text.matches("([0-9]*)")) {
                if (!text.isEmpty()) {
                    try {
                        int value = Integer.parseInt(text);
                        if (value <= 3000) {
                            return change;
                        }
                    } catch (NumberFormatException e) {
                        return null;
                    }
                } else {
                    return change;
                }
            }
            return null;
        };

        widthField.setTextFormatter(new TextFormatter<>(filter));
        heightField.setTextFormatter(new TextFormatter<>(filter));

        Button btnZmien = new Button("Zmień rozmiar");
        Button btnAnuluj = new Button("Anuluj");
        Button btnReset = new Button("Przywróć oryginalne");

        btnAnuluj.setOnAction(e -> {
            widthField.clear();
            heightField.clear();
            modalStage.hide();
        });

        btnReset.setOnAction(e -> {
            if (org.getImage() != null) {
                widthField.setText(String.valueOf((int)org.getImage().getWidth()));
                heightField.setText(String.valueOf((int)org.getImage().getHeight()));
                widthError.setText("");
                heightError.setText("");
            }
        });

        btnZmien.setOnAction(e -> {
            boolean valid = true;
            widthError.setText("");
            heightError.setText("");

            if (widthField.getText().isEmpty()) {
                widthError.setText("Pole jest wymagane");
                valid = false;
            }
            if (heightField.getText().isEmpty()) {
                heightError.setText("Pole jest wymagane");
                valid = false;
            }

            if (valid) {
                int newWidth = Integer.parseInt(widthField.getText());
                int newHeight = Integer.parseInt(heightField.getText());

                Image baseImage = mod.getImage() != null ? mod.getImage() : org.getImage();
                if (baseImage != null) {
                    ImageView tempView = new ImageView(baseImage);
                    tempView.setFitWidth(newWidth);
                    tempView.setFitHeight(newHeight);
                    tempView.setPreserveRatio(false);
                    
                    Image scaledImage = tempView.snapshot(null, null);
                    mod.setImage(scaledImage);
                }
                modalStage.hide();
            }
        });

        HBox btnBox = new HBox(10, btnZmien, btnAnuluj, btnReset);
        btnBox.setAlignment(Pos.CENTER);

        vbox.getChildren().addAll(
            widthBox, widthError, 
            heightBox, heightError, 
            btnBox
        );

        modalStage.setScene(new Scene(vbox));
        modalStage.show();
    }

    @FXML
    void zapisz() {
        Stage ownerStage = (Stage) operacje.getScene().getWindow();
        FileChooser fc = new FileChooser();
        fc.setTitle("Zapisz obraz");
        fc.getExtensionFilters().add(new FileChooser.ExtensionFilter("Plik JPG", "*.jpg"));

        File fileToSave = fc.showSaveDialog(ownerStage);
        if (fileToSave != null) {
            String fileName = fileToSave.getName();
            if (!fileName.toLowerCase().endsWith(".jpg")) {
                fileToSave = new File(fileToSave.getParentFile(), fileName + ".jpg");
                fileName = fileToSave.getName();
            }

            try {
                Image imageToSave = mod.getImage() != null ? mod.getImage() : org.getImage();

                if (imageToSave != null) {
                    BufferedImage argbImage = SwingFXUtils.fromFXImage(imageToSave, null);

                    BufferedImage rgbImage = new BufferedImage(
                            argbImage.getWidth(),
                            argbImage.getHeight(),
                            BufferedImage.TYPE_INT_RGB
                    );

                    java.awt.Graphics2D graphics = rgbImage.createGraphics();
                    graphics.drawImage(argbImage, 0, 0, java.awt.Color.WHITE, null);
                    graphics.dispose();

                    ImageIO.write(rgbImage, "jpg", fileToSave);
                    showToast("Zapisano obraz w pliku " + fileName);
                }
            } catch (Exception e) {
                showToast("Nie udało się zapisać pliku " + fileToSave.getName());
            }
        }
    }
    void showToast(String message){
        Stage ownerStage = (Stage) operacje.getScene().getWindow();
        Popup popup = new Popup();
        popup.setAutoFix(true);
        popup.setHideOnEscape(true);

        Label label = new Label(message);
        popup.getContent().add(label);
        popup.setOnShown(e -> {
            popup.setX(ownerStage.getX() + ownerStage.getWidth() / 2 - popup.getWidth() / 2);
            popup.setY(ownerStage.getY() + ownerStage.getHeight() - 100);
        });
        popup.show(ownerStage);
        PauseTransition delay = new PauseTransition(Duration.seconds(2.5));
        delay.setOnFinished(e -> popup.hide());
        delay.play();
    }

}