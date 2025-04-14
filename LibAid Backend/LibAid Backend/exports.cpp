#include "mainHeader.h"

static HashTable ht;
static SnapshotStack* undoStack = nullptr;

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
        newUser->next = nullptr;

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

        Book* newBook = (Book*)malloc(sizeof(Book));
        if (!newBook) return;

        int hash = generateBookHash(title);

        strcpy_s(newBook->title, MAX_TITLE_LEN, title);
        strcpy_s(newBook->author, MAX_AUTHOR_LEN, author);
        newBook->hashCode = hash;
        newBook->borrowedBy = nullptr;
        newBook->queueFront = nullptr;
        newBook->queueRear = nullptr;
        newBook->next = nullptr;

        Book* current = ht.table[hash];
        while (current) {
            if (strcmp(current->title, title) == 0 && current->hashCode == hash) {
                free(newBook);
                return;
            }
            current = current->next;
        }

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
        if (!user) return;

        int bookHash = generateBookHash(bookTitle);
        Book* book = searchBookByHash(&ht, bookHash);
        if (!book) return;

        if (book->borrowedBy) {
            enqueueUser(book, user);
        }
        else {
            book->borrowedBy = user;
        }

        syncDatabaseToFile(&ht, "database.txt");
    }

    __declspec(dllexport) void ReturnBook(const char* bookTitle) {
        pushSnapshot(&ht, undoStack);

        int bookHash = generateBookHash(bookTitle);
        Book* book = searchBookByHash(&ht, bookHash);
        if (!book || !book->borrowedBy) return;

        book->borrowedBy = nullptr;

        User* nextUser = dequeueUser(book);
        if (nextUser) {
            book->borrowedBy = nextUser;
        }

        syncDatabaseToFile(&ht, "database.txt");
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
    __declspec(dllexport) User* SearchUserByLastName(const char* lastName) {
        int hash = generateUserHash(lastName);
        return searchUserByHash(&ht, hash);
    }

    __declspec(dllexport) Book* SearchBookByTitle(const char* title) {
        int hash = generateBookHash(title);
        return searchBookByHash(&ht, hash);
    }
    __declspec(dllexport) void RemoveUser(const char* lastName) {
        pushSnapshot(&ht, undoStack);
        
    }

    __declspec(dllexport) void RemoveBook(const char* title) {
        pushSnapshot(&ht, undoStack);
        
    }

    __declspec(dllexport) void UpdateUser(const char* lastName, const char* newFirstName, const char* newLastName) {
        pushSnapshot(&ht, undoStack);
        
    }

    __declspec(dllexport) void UpdateBook(const char* title, const char* newTitle, const char* newAuthor) {
        pushSnapshot(&ht, undoStack);
        
    }
    __declspec(dllexport) void PrintLogs() {
        printLogs(); 
    }




}
