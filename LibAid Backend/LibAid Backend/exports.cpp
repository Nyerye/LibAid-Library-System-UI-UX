#include "mainHeader.h"

static HashTable ht;
static SnapshotStack* undoStack = nullptr;

Book* findBookByTitle(const char* title) {
    int hash = generateBookHash(title);
    Book* current = ht.table[hash];
    while (current) {
        if (!current->isDeleted && strcmp(current->title, title) == 0) {
            return current;
        }
        current = current->next;
    }
    return nullptr;
}

extern "C" {

    __declspec(dllexport) void InitSystem() {
        initHashTable(&ht);
        undoStack = initSnapshotStack();
        loadDatabase(&ht, "database.txt");
    }

    __declspec(dllexport) void CleanupSystem() {
        freeSnapshotStack(&undoStack);
        freeHashTable(&ht);
    }

    __declspec(dllexport) void AddUser(const char* firstName, const char* lastName) {
        pushSnapshot(&ht, undoStack);

        User* newUser = (User*)malloc(sizeof(User));
        if (!newUser) return;

        strcpy_s(newUser->firstName, MAX_NAME_LEN, firstName);
        strcpy_s(newUser->lastName, MAX_NAME_LEN, lastName);
        newUser->userId = generateUserHash(lastName);
        newUser->isDeleted = false;
        int index = newUser->userId % TABLE_SIZE;

        newUser->next = ht.users[index];
        ht.users[index] = newUser;

        char logMsg[MAX_LOG_LEN];
        snprintf(logMsg, sizeof(logMsg), "User %s %s added via GUI", firstName, lastName);
        logAction("Add User", logMsg);
        syncDatabaseToFile(&ht, "database.txt");
    }

    __declspec(dllexport) void AddBook(const char* title, const char* author) {
        pushSnapshot(&ht, undoStack);

        int hash = generateBookHash(title);
        Book* current = ht.table[hash];
        while (current) {
            if (!current->isDeleted && strcmp(current->title, title) == 0) return;
            current = current->next;
        }

        Book* newBook = (Book*)malloc(sizeof(Book));
        if (!newBook) return;

        strcpy_s(newBook->title, MAX_TITLE_LEN, title);
        strcpy_s(newBook->author, MAX_AUTHOR_LEN, author);
        newBook->hashCode = hash;
        newBook->isBorrowed = false;
        newBook->borrowedById = -1;
        newBook->isDeleted = false;
        newBook->queueFront = nullptr;
        newBook->queueRear = nullptr;
        newBook->next = ht.table[hash];
        ht.table[hash] = newBook;

        char logMsg[MAX_LOG_LEN];
        snprintf(logMsg, sizeof(logMsg), "Book '%s' by '%s' added via GUI", title, author);
        logAction("Add Book", logMsg);
        syncDatabaseToFile(&ht, "database.txt");
    }

    __declspec(dllexport) void BorrowBook(const char* userLastName, const char* bookTitle) {
        pushSnapshot(&ht, undoStack);

        int userHash = generateUserHash(userLastName);
        User* user = searchUserByHash(&ht, userHash);
        if (!user || user->isDeleted) return;

        Book* book = findBookByTitle(bookTitle);
        if (!book || book->isDeleted) return;

        if (book->isBorrowed) {
            enqueueUser(book, user);
        }
        else {
            book->isBorrowed = true;
            book->borrowedById = user->userId;
        }

        syncDatabaseToFile(&ht, "database.txt");
    }

    __declspec(dllexport) void ReturnBook(const char* bookTitle) {
        pushSnapshot(&ht, undoStack);

        Book* book = findBookByTitle(bookTitle);
        if (!book || book->isDeleted || !book->isBorrowed) return;

        book->isBorrowed = false;
        book->borrowedById = -1;

        User* nextUser = dequeueUser(book);
        if (nextUser) {
            book->isBorrowed = true;
            book->borrowedById = nextUser->userId;
        }

        syncDatabaseToFile(&ht, "database.txt");
    }

    __declspec(dllexport) void RemoveUser(const char* lastName) {
        pushSnapshot(&ht, undoStack);

        int hash = generateUserHash(lastName);
        User* user = searchUserByHash(&ht, hash);
        if (!user) return;

        user->isDeleted = true;

        char logMsg[MAX_LOG_LEN];
        snprintf(logMsg, sizeof(logMsg), "User %s removed via GUI", lastName);
        logAction("Remove User", logMsg);
        syncDatabaseToFile(&ht, "database.txt");
    }

    __declspec(dllexport) void RemoveBook(const char* title) {
        pushSnapshot(&ht, undoStack);

        Book* book = findBookByTitle(title);
        if (!book) return;

        book->isDeleted = true;

        char logMsg[MAX_LOG_LEN];
        snprintf(logMsg, sizeof(logMsg), "Book %s removed via GUI", title);
        logAction("Remove Book", logMsg);
        syncDatabaseToFile(&ht, "database.txt");
    }

    __declspec(dllexport) void UpdateUser(const char* oldLastName, const char* newFirstName, const char* newLastName) {
        pushSnapshot(&ht, undoStack);

        int hash = generateUserHash(oldLastName);
        User* user = searchUserByHash(&ht, hash);
        if (!user || user->isDeleted) return;

        if (strlen(newFirstName) > 0)
            strcpy_s(user->firstName, MAX_NAME_LEN, newFirstName);

        if (strlen(newLastName) > 0 && strcmp(newLastName, oldLastName) != 0) {
            int index = hash % TABLE_SIZE;
            User* prev = nullptr;
            User* current = ht.users[index];
            while (current) {
                if (current == user) break;
                prev = current;
                current = current->next;
            }

            if (current) {
                if (prev == nullptr) ht.users[index] = current->next;
                else prev->next = current->next;
            }

            strcpy_s(user->lastName, MAX_NAME_LEN, newLastName);
            user->userId = generateUserHash(newLastName);
            int newIndex = user->userId % TABLE_SIZE;
            user->next = ht.users[newIndex];
            ht.users[newIndex] = user;
        }

        syncDatabaseToFile(&ht, "database.txt");
    }

    __declspec(dllexport) void UpdateBook(const char* oldTitle, const char* newTitle, const char* newAuthor) {
        pushSnapshot(&ht, undoStack);

        Book* book = findBookByTitle(oldTitle);
        if (!book || book->isDeleted) return;

        if (strlen(newAuthor) > 0)
            strcpy_s(book->author, MAX_AUTHOR_LEN, newAuthor);

        if (strlen(newTitle) > 0 && strcmp(newTitle, oldTitle) != 0) {
            int oldHash = generateBookHash(oldTitle);
            int index = oldHash % TABLE_SIZE;

            Book* prev = nullptr;
            Book* current = ht.table[index];
            while (current) {
                if (current == book) break;
                prev = current;
                current = current->next;
            }

            if (current) {
                if (prev == nullptr)
                    ht.table[index] = current->next;
                else
                    prev->next = current->next;
            }

            strcpy_s(book->title, MAX_TITLE_LEN, newTitle);
            book->hashCode = generateBookHash(newTitle);
            int newIndex = book->hashCode % TABLE_SIZE;
            book->next = ht.table[newIndex];
            ht.table[newIndex] = book;
        }

        syncDatabaseToFile(&ht, "database.txt");
    }

    __declspec(dllexport) void HardDeleteUser(const char* lastName) {
        int hash = generateUserHash(lastName);
        int index = hash % TABLE_SIZE;

        User* current = ht.users[index];
        User* prev = nullptr;
        pushSnapshot(&ht, undoStack);

        while (current) {
            if (current->userId == hash && strcmp(current->lastName, lastName) == 0) {
                if (prev == nullptr)
                    ht.users[index] = current->next;
                else
                    prev->next = current->next;

                char logMsg[MAX_LOG_LEN];
                snprintf(logMsg, sizeof(logMsg), "User %s hard-deleted via GUI", lastName);
                logAction("Hard Delete User", logMsg);
                free(current);

                syncDatabaseToFile(&ht, "database.txt");
                return;
            }
            prev = current;
            current = current->next;
        }
    }
    __declspec(dllexport) void HardDeleteBook(const char* title) {
        int hash = generateBookHash(title);
        int index = hash % TABLE_SIZE;

        Book* current = ht.table[index];
        Book* prev = nullptr;
        pushSnapshot(&ht, undoStack);

        while (current) {
            if (strcmp(current->title, title) == 0 && current->hashCode == hash) {
                if (prev == nullptr)
                    ht.table[index] = current->next;
                else
                    prev->next = current->next;

                char logMsg[MAX_LOG_LEN];
                snprintf(logMsg, sizeof(logMsg), "Book %s hard-deleted via GUI", title);
                logAction("Hard Delete Book", logMsg);
                free(current);

                syncDatabaseToFile(&ht, "database.txt");
                return;
            }
            prev = current;
            current = current->next;
        }
    }


    __declspec(dllexport) bool UserExists(const char* lastName) {
        int hash = generateUserHash(lastName);
        User* user = searchUserByHash(&ht, hash);
        return user && !user->isDeleted;
    }

    __declspec(dllexport) bool BookExists(const char* title) {
        Book* book = findBookByTitle(title);
        return book && !book->isDeleted;
    }

    __declspec(dllexport) void UndoLastAction() {
        undo_last_action(&ht, undoStack);
    }

    __declspec(dllexport) void PrintUsers() {
        printUsers(&ht);
    }

    __declspec(dllexport) void PrintBooks() {
        printBooks(&ht);
    }
}
