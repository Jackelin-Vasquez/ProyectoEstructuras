import csv
import os
import tkinter as tk
from tkinter import messagebox, ttk

CATEGORIAS = {
    0: "Tecnologia",
    1: "Ciencia",
    2: "Literatura",
    3: "Historia",
    4: "Ficción",
    5: "Otro",
}


class BibliotecaApp:

  def __init__(self, root):
    self.root = root
    self.root.title("Sistema de Gestión de Biblioteca")
    self.root.geometry("1000x600")
    self.root.config(bg="#f4f6f9")

    # Ruta absoluta para asegurar que encuentre el archivo en la misma carpeta del script
    script_dir =os.path.dirname(os.path.abspath(__file__))
    self.archivo = os.path.join(
        script_dir, "bin", "Debug", "net10.0", "biblioteca.csv"
    )
    # Estilo general
    self.style = ttk.Style()
    self.style.theme_use("clam")

    self.crear_interfaz()
    self.cargar_datos()

  def crear_interfaz(self):
    # Título principal
    titulo_lbl = tk.Label(
        self.root,
        text="GESTIÓN DE BIBLIOTECA",
        font=("Arial", 16, "bold"),
        bg="#f4f6f9",
        fg="#333333",
    )
    titulo_lbl.pack(pady=10)

    # Contenedor principal dividido en dos paneles
    main_frame = tk.Frame(self.root, bg="#f4f6f9")
    main_frame.pack(fill=tk.BOTH, expand=True, padx=15, pady=5)

    # --- PANEL IZQUIERDO (Formulario y Controles) ---
    left_frame = tk.LabelFrame(
        main_frame,
        text=" Panel de Control ",
        font=("Arial", 11, "bold"),
        bg="#ffffff",
        fg="#333333",
        padx=10,
        pady=10,
    )
    left_frame.pack(side=tk.LEFT, fill=tk.Y, padx=(0, 10))

    # Entradas de formulario
    tk.Label(
        left_frame, text="Código:", bg="#ffffff", font=("Arial", 9, "bold")
    ).pack(anchor="w", pady=(5, 0))
    self.txt_codigo = tk.Entry(left_frame, font=("Arial", 10), width=25)
    self.txt_codigo.pack(pady=(0, 5))

    tk.Label(
        left_frame, text="Título:", bg="#ffffff", font=("Arial", 9, "bold")
    ).pack(anchor="w", pady=(5, 0))
    self.txt_titulo = tk.Entry(left_frame, font=("Arial", 10), width=25)
    self.txt_titulo.pack(pady=(0, 5))

    tk.Label(
        left_frame, text="Autor:", bg="#ffffff", font=("Arial", 9, "bold")
    ).pack(anchor="w", pady=(5, 0))
    self.txt_autor = tk.Entry(left_frame, font=("Arial", 10), width=25)
    self.txt_autor.pack(pady=(0, 5))

    tk.Label(
        left_frame, text="Categoría:", bg="#ffffff", font=("Arial", 9, "bold")
    ).pack(anchor="w", pady=(5, 0))
    self.cmb_categoria = ttk.Combobox(
        left_frame,
        values=list(CATEGORIAS.values()),
        state="readonly",
        width=23,
        font=("Arial", 10),
    )
    self.cmb_categoria.pack(pady=(0, 5))
    self.cmb_categoria.current(0)

    tk.Label(
        left_frame,
        text="Copias Disponibles:",
        bg="#ffffff",
        font=("Arial", 9, "bold"),
    ).pack(anchor="w", pady=(5, 0))
    self.txt_copias = tk.Entry(left_frame, font=("Arial", 10), width=25)
    self.txt_copias.pack(pady=(0, 15))

    # Botones de acción
    btn_agregar = tk.Button(
        left_frame,
        text="Registrar Libro",
        command=self.registrar_libro,
        bg="#4CAF50",
        fg="white",
        font=("Arial", 9, "bold"),
        width=22,
    )
    btn_agregar.pack(pady=5)

    btn_prestar = tk.Button(
        left_frame,
        text="Registrar Préstamo",
        command=self.registrar_prestamo,
        bg="#2196F3",
        fg="white",
        font=("Arial", 9, "bold"),
        width=22,
    )
    btn_prestar.pack(pady=5)

    btn_devolver = tk.Button(
        left_frame,
        text="Registrar Devolución",
        command=self.registrar_devolucion,
        bg="#FF9800",
        fg="white",
        font=("Arial", 9, "bold"),
        width=22,
    )
    btn_devolver.pack(pady=5)

    btn_recargar = tk.Button(
        left_frame,
        text="Actualizar Vista",
        command=self.cargar_datos,
        bg="#607D8B",
        fg="white",
        font=("Arial", 9, "bold"),
        width=22,
    )
    btn_recargar.pack(pady=(15, 5))

    # --- PANEL DERECHO (Tabla / Catálogo) ---
    right_frame = tk.Frame(main_frame, bg="#f4f6f9")
    right_frame.pack(side=tk.RIGHT, fill=tk.BOTH, expand=True)

    # Barra de búsqueda
    search_frame = tk.Frame(right_frame, bg="#f4f6f9")
    search_frame.pack(fill=tk.X, pady=(0, 10))

    tk.Label(
        search_frame, text="Buscar:", bg="#f4f6f9", font=("Arial", 9, "bold")
    ).pack(side=tk.LEFT, padx=(0, 5))
    self.txt_buscar = tk.Entry(search_frame, font=("Arial", 10), width=30)
    self.txt_buscar.pack(side=tk.LEFT, padx=(0, 5))
    self.txt_buscar.bind("<KeyRelease>", self.filtrar_datos)

    # Tabla Treeview
    columnas = (
        "Código",
        "Título",
        "Autor",
        "Categoría",
        "Disponibles",
        "Prestados",
    )
    self.tree = ttk.Treeview(right_frame, columns=columnas, show="headings")

    for col in columnas:
      self.tree.heading(col, text=col)
      self.tree.column(col, width=100, anchor=tk.CENTER)

    self.tree.column("Título", width=180, anchor=tk.W)
    self.tree.column("Autor", width=140, anchor=tk.W)

    # Scrollbar para la tabla
    scrollbar = ttk.Scrollbar(
        right_frame, orient=tk.VERTICAL, command=self.tree.yview
    )
    self.tree.configure(yscroll=scrollbar.set)

    self.tree.pack(side=tk.LEFT, fill=tk.BOTH, expand=True)
    scrollbar.pack(side=tk.RIGHT, fill=tk.Y)

  def leer_csv_completo(self):
    filas = []
    if os.path.exists(self.archivo):
      with open(self.archivo, mode="r", encoding="utf-8") as f:
        lector = csv.reader(f)
        for fila in lector:
          if fila:
            filas.append(fila)
    return filas

  def escribir_csv_completo(self, filas):
    with open(self.archivo, mode="w", newline="", encoding="utf-8") as f:
      escritor = csv.writer(f)
      escritor.writerows(filas)

  def cargar_datos(self):
    for item in self.tree.get_children():
      self.tree.delete(item)

    filas = self.leer_csv_completo()
    for fila in filas:
      if len(fila) == 6:
        try:
          cat_id = int(fila[3])
          fila[3] = CATEGORIAS.get(cat_id, "Desconocida")
        except ValueError:
          pass
        self.tree.insert("", tk.END, values=fila)

  def filtrar_datos(self, event):
    query = self.txt_buscar.get().lower()
    for item in self.tree.get_children():
      self.tree.delete(item)

    filas = self.leer_csv_completo()
    for fila in filas:
      if len(fila) == 6:
        if (
            query in fila[0].lower()
            or query in fila[1].lower()
            or query in fila[2].lower()
        ):
          try:
            cat_id = int(fila[3])
            fila[3] = CATEGORIAS.get(cat_id, "Desconocida")
          except ValueError:
            pass
          self.tree.insert("", tk.END, values=fila)

  def registrar_libro(self):
    codigo = self.txt_codigo.get().strip()
    titulo = self.txt_titulo.get().strip()
    autor = self.txt_autor.get().strip()
    categoria_nombre = self.cmb_categoria.get()
    copias = self.txt_copias.get().strip()

    if not codigo or not titulo or not autor or not copias:
      messagebox.showerror("Error", "Todos los campos son obligatorios.")
      return

    try:
      int(codigo)
      int(copias)
    except ValueError:
      messagebox.showerror(
          "Error", "El código y las copias deben ser números enteros."
      )
      return

    cat_id = 0
    for k, v in CATEGORIAS.items():
      if v == categoria_nombre:
        cat_id = k
        break

    filas = self.leer_csv_completo()
    for fila in filas:
      if fila[0] == codigo:
        messagebox.showerror("Error", f"El código {codigo} ya está registrado.")
        return

    nueva_fila = [codigo, titulo, autor, str(cat_id), copias, "0"]
    filas.append(nueva_fila)
    self.escribir_csv_completo(filas)

    messagebox.showinfo("Éxito", "Libro registrado correctamente.")
    self.limpiar_formulario()
    self.cargar_datos()

  def registrar_prestamo(self):
    seleccion = self.tree.selection()
    if not seleccion:
      messagebox.showwarning(
          "Advertencia",
          "Por favor, seleccione un libro de la tabla para el préstamo.",
      )
      return

    item = self.tree.item(seleccion)
    codigo_seleccionado = item["values"][0]

    filas = self.leer_csv_completo()
    for fila in filas:
      if str(fila[0]) == str(codigo_seleccionado):
        copias_disp = int(fila[4])
        if copias_disp > 0:
          fila[4] = str(copias_disp - 1)
          fila[5] = str(int(fila[5]) + 1)
          self.escribir_csv_completo(filas)
          messagebox.showinfo(
              "Éxito", f"Préstamo registrado para '{fila[1]}'."
          )
          self.cargar_datos()
          return
        else:
          messagebox.showerror("Error", "No hay copias disponibles.")
          return

  def registrar_devolucion(self):
    seleccion = self.tree.selection()
    if not seleccion:
      messagebox.showwarning(
          "Advertencia",
          "Por favor, seleccione un libro de la tabla para la devolución.",
      )
      return

    item = self.tree.item(seleccion)
    codigo_seleccionado = item["values"][0]

    filas = self.leer_csv_completo()
    for fila in filas:
      if str(fila[0]) == str(codigo_seleccionado):
        fila[4] = str(int(fila[4]) + 1)
        self.escribir_csv_completo(filas)
        messagebox.showinfo(
            "Éxito", f"Devolución registrada correctamente para '{fila[1]}'."
        )
        self.cargar_datos()
        return

  def limpiar_formulario(self):
    self.txt_codigo.delete(0, tk.END)
    self.txt_titulo.delete(0, tk.END)
    self.txt_autor.delete(0, tk.END)
    self.txt_copias.delete(0, tk.END)
    self.cmb_categoria.current(0)


if __name__ == "__main__":
  root = tk.Tk()
  app = BibliotecaApp(root)
  root.mainloop()