module com.example.java_gui {
    requires javafx.controls;
    requires javafx.fxml;
    requires java.desktop;
    requires javafx.swing;


    opens com.example.java_gui to javafx.fxml;
    exports com.example.java_gui;
}